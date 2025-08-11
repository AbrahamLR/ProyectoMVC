using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using udemyMVC.Models;
using udemyMVC.ClasesAuxiliares;

namespace udemyMVC.Controllers
{
    public class LoginController : Controller
    {
        // (master/admin)
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public string Login(UsuarioCLS oUsuarioCLS)
        {
            string resp = "";
            string nombreUsuario = oUsuarioCLS.NomUsuario;
            string password = oUsuarioCLS.contrasena;
            SHA256Managed sha = new SHA256Managed();
            byte[] bytePass = Encoding.Default.GetBytes(password);
            byte[] bytePassEncri = sha.ComputeHash(bytePass);
            string passEncry = BitConverter.ToString(bytePassEncri).Replace("-", "");

            using (var bd = new BDPasajeEntities())
            {
                if (!ModelState.IsValid)
                {
                    var query = (from state in ModelState.Values
                                 from error in state.Errors
                                 select error.ErrorMessage).ToList();
                    resp += "<ul class='list-group'>";
                    foreach (var item in query)
                    {
                        resp += "<li class='list-group-item'>" + item + "</li>";
                    }
                    resp += "</ul>";
                }
                else
                {
                    int registros = 0;
                    registros = bd.Usuario.Where(p => p.NOMBREUSUARIO == nombreUsuario && p.CONTRA == passEncry).Count();
                    resp = registros.ToString();
                    if (resp == "0") resp = "Usuario o contraseña incorrecta";
                    else
                    {
                        Usuario oUsuario= bd.Usuario.Where(p => p.NOMBREUSUARIO == nombreUsuario && p.CONTRA == passEncry).First();
                        Session["Usuario"] = oUsuario;
                        List<MenuCLS> listaMenu = (from Usuario in bd.Usuario
                                                   join Rol in bd.Rol
                                                   on Usuario.IIDROL equals Rol.IIDROL
                                                   join RP in bd.RolPagina
                                                   on Rol.IIDROL equals RP.IIDROL
                                                   join pag in bd.Pagina
                                                   on RP.IIDPAGINA equals pag.IIDPAGINA
                                                   where Rol.IIDROL == oUsuario.IIDROL && RP.IIDROL == oUsuario.IIDROL && Usuario.IID==oUsuario.IID
                                                   select new MenuCLS
                                                   {
                                                       nomAccion = pag.ACCION,
                                                       nomControlado = pag.CONTROLADOR,
                                                       mensaje = pag.MENSAJE
                                                   }).ToList();
                        Session["Rol"]= listaMenu;
                       

                    }
                }
            }

                return resp;
        }

        public string RecuperarContra(string IIDTipo,string correo,string telefono)
        {
            string mensaje = "";
            using (var bd = new BDPasajeEntities())
            {
                
                int cantidad = 0;
                int idCliente;
                if (IIDTipo == "C")
                {
                    cantidad = bd.Cliente.Where(p => p.EMAIL == correo && p.TELEFONOCELULAR == telefono).Count();
                }
                if (cantidad == 0)
                {
                    mensaje = "No existe registro";
                }
                else
                {
                    idCliente = bd.Cliente.Where(p => p.EMAIL == correo && p.TELEFONOCELULAR == telefono).First().IIDCLIENTE;
                    int cant = bd.Usuario.Where(p => p.IID == idCliente && p.TIPOUSUARIO == "C").Count();
                    if (cant == 0) { mensaje = "No tiene usuario";
                    }
                    else {
                        Usuario ousuario = bd.Usuario.Where(p => p.IID == idCliente && p.TIPOUSUARIO == "C").First();
                        string nuevaContra = GenerateRandomPassword(8);
                        Random ra = new Random();
                        //int n1= ra.Next(0, 9);
                        //int n2 = ra.Next(0, 9);
                        //int n3 = ra.Next(0, 9);
                        //int n4 = ra.Next(0, 9);
                        //string nuevaContra = n1.ToString() + n2.ToString() + n3.ToString() + n4.ToString();
                        SHA256Managed sha = new SHA256Managed();
                        byte[] bytePass = Encoding.Default.GetBytes(nuevaContra);
                        byte[] bytePassEncri = sha.ComputeHash(bytePass);
                        string passEncry = BitConverter.ToString(bytePassEncri).Replace("-", "");
                        ousuario.CONTRA = passEncry;
                        mensaje = bd.SaveChanges().ToString();
                        Correo.enviarCorreo(correo,"Reestableciomiento de contraseña","Se genero una nueva contraseña, su nueva contraseña es:"+nuevaContra, @"C:\Users\abrah\source\repos\udemyMVC\udemyMVC\Archivos\LogCorreo.txt");
                    }
                }               
            }
            return mensaje;
        }

        private string GenerateRandomPassword(int length)
        {
            const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
            const string numberChars = "0123456789";
            const string symbolChars = "!@#$&*";

            Random random = new Random();
            StringBuilder password = new StringBuilder();

            // Asegurar al menos un carácter de cada tipo
            password.Append(upperChars[random.Next(upperChars.Length)]);
            password.Append(lowerChars[random.Next(lowerChars.Length)]);
            password.Append(numberChars[random.Next(numberChars.Length)]);
            password.Append(symbolChars[random.Next(symbolChars.Length)]);

            // Completar el resto de la contraseña con caracteres aleatorios
            string allChars = upperChars + lowerChars + numberChars + symbolChars;
            for (int i = 4; i < length; i++)
            {
                password.Append(allChars[random.Next(allChars.Length)]);
            }

            // Mezclar la contraseña para evitar patrones predecibles
            return ShuffleString(password.ToString(), random);
        }

        private string ShuffleString(string str, Random random)
        {
            char[] array = str.ToCharArray();
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                // Intercambio de valores sin usar tuplas
                char temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
            return new string(array);
        }
        //private string ShuffleString(string str, Random random)
        //{
        //    char[] array = str.ToCharArray();
        //    for (int i = array.Length - 1; i > 0; i--)
        //    {
        //        int j = random.Next(i + 1);
        //        (array[i], array[j]) = (array[j], array[i]);
        //    }
        //    return new string(array);
        //}

        public ActionResult Logout()
        {
            Session["Usuario"] = null;
            Session["Rol"] = null; 
            return RedirectToAction("Index");
        }
    }
}