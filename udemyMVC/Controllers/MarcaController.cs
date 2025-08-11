using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using udemyMVC.Models; //para poder usar EF
using udemyMVC.Filtros;

namespace udemyMVC.Controllers
{
    [Acceder]
    public class MarcaController : Controller
    {       
        public FileResult generarExcel()
        {
            byte[] buffer;
            using (MemoryStream ms = new MemoryStream())
            {
                ExcelPackage ep = new ExcelPackage();
                //Crear hoja
                ep.Workbook.Worksheets.Add("Reporte");
                ExcelWorksheet ew = ep.Workbook.Worksheets[0];
                ew.Cells[1, 1].Value = "Id Marca";
                ew.Cells[1, 2].Value = "Nombre Marca";
                ew.Cells[1, 3].Value = "Descripcion Marca";
                ew.Column(1).Width = 20;
                ew.Column(2).Width = 40;
                ew.Column(3).Width = 180;
                using (var range = ew.Cells[1, 1, 1, 3])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkRed);
                }
                List<MarcaCLS> lista = (List<MarcaCLS>)Session["listaMarcas"];
                int numFila = lista.Count;
                for (int i = 0; i < numFila; i++)
                {
                    ew.Cells[i + 2, 1].Value = lista[i].idmarca;
                    ew.Cells[i + 2, 2].Value = lista[i].nombre;
                    ew.Cells[i + 2, 3].Value = lista[i].descripcion;
                }
                ep.SaveAs(ms);
                buffer = ms.ToArray();

            }
            return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
        public FileResult generaPDF()
        {
            Document doc = new Document();
            byte[] buffer;
            using (MemoryStream ms = new MemoryStream())
            {
                //Guarda el documento en memoria
                PdfWriter.GetInstance(doc, ms);
                doc.Open();
                //Columnas
                Paragraph title = new Paragraph("Listado de marcas");                            
                title.Alignment = Element.ALIGN_CENTER;
                doc.Add(title);
                Paragraph esp = new Paragraph(" ");
                doc.Add(esp);
                PdfPTable table = new PdfPTable(3);
                float[] values = new float[3] {30,40,80 };
                table.SetWidths(values);
                //Celdas
                PdfPCell c1 = new PdfPCell(new Paragraph("Id Marca"));
                c1.BackgroundColor = new BaseColor(130,130,130);
                c1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(c1);
                PdfPCell c2 = new PdfPCell(new Paragraph("Nombre"));
                c2.BackgroundColor = new BaseColor(130, 130, 130);
                c2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(c2);
                PdfPCell c3 = new PdfPCell(new Paragraph("Descripcion"));
                c3.BackgroundColor = new BaseColor(130, 130, 130);
                c3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(c3);
                List<MarcaCLS> lista = (List<MarcaCLS>)Session["listaMarcas"];
                int numFilas = lista.Count;
                for (int i = 0; i < numFilas; i++)
                {
                    table.AddCell(lista[i].idmarca.ToString());
                    table.AddCell(lista[i].nombre);
                    table.AddCell(lista[i].descripcion);
                }
                doc.Add(table);
                doc.Close();
                buffer = ms.ToArray();
                

            }
            return File(buffer, "application/pdf");
        }
        // GET: Marca
        public ActionResult Index(MarcaCLS oMarcaCls)
        {
            string nombreMarca = oMarcaCls.nombre;
            List<MarcaCLS> listaMarca = null; //simpre fuera del using para poder retornarla
            using (var bd = new BDPasajeEntities())
            {
                if (oMarcaCls.nombre == null)
                {
                    listaMarca = (from marca in bd.Marca
                                  where marca.BHABILITADO == 1
                                  select new MarcaCLS
                                  {
                                      idmarca = marca.IIDMARCA,
                                      nombre = marca.NOMBRE,
                                      descripcion = marca.DESCRIPCION
                                  }).ToList();
                    Session["listaMarcas"] = listaMarca;
                }
                else
                {
                    listaMarca = (from marca in bd.Marca
                                  where marca.BHABILITADO == 1 && marca.NOMBRE.Contains(nombreMarca)
                                  select new MarcaCLS
                                  {
                                      idmarca = marca.IIDMARCA,
                                      nombre = marca.NOMBRE,
                                      descripcion = marca.DESCRIPCION
                                  }).ToList();
                    Session["listaMarcas"] = listaMarca;
                }
                
            }
            return View(listaMarca);
        }

        public ActionResult Agregar()
        {           
           return View();            
          
        }
        [HttpPost]
        public ActionResult Agregar(MarcaCLS oMarcaCLS)
        {
            int cont = 0;
            string nom = oMarcaCLS.nombre;
            using (var bd = new BDPasajeEntities())
            {
                cont = bd.Marca.Where(p => p.NOMBRE.Equals(nom)).Count();
            }

                if (!ModelState.IsValid||cont>0)
                {
                    if (cont > 0) { oMarcaCLS.mensajeError = "Marca ya registrada"; }
                    return View(oMarcaCLS);
                }
                else
                {
                    using (var bd = new BDPasajeEntities())
                    {
                        Marca oMarca = new Marca();
                        oMarca.NOMBRE = oMarcaCLS.nombre;
                        oMarca.DESCRIPCION = oMarcaCLS.descripcion;
                        oMarca.BHABILITADO = 1;
                        bd.Marca.Add(oMarca);
                        bd.SaveChanges();

                    }
                    return RedirectToAction("Index");
                }
        }

        public ActionResult Editar(int id)
        {
            MarcaCLS oMarcaCLS = new MarcaCLS();
            using (var bd = new BDPasajeEntities())
            {
                Marca oMarca = bd.Marca.Where(p => p.IIDMARCA.Equals(id)).First();
                oMarcaCLS.idmarca = oMarca.IIDMARCA;
                oMarcaCLS.nombre = oMarca.NOMBRE;
                oMarcaCLS.descripcion = oMarca.DESCRIPCION;

            }
                return View(oMarcaCLS);
        }

        [HttpPost]
        public ActionResult Editar(MarcaCLS oMarcaCLS)
        {
            int cont = 0;
            int idMa = oMarcaCLS.idmarca;
            string nom = oMarcaCLS.nombre;
            using (var bd = new BDPasajeEntities())
            {
                cont = bd.Marca.Where(p => p.NOMBRE.Equals(nom) && !p.IIDMARCA.Equals(idMa)).Count();
            }
            if (!ModelState.IsValid || cont > 0) { if (cont > 0) { oMarcaCLS.mensajeError = "Marca ya registrada"; }  return View(oMarcaCLS); }
            int idMarca = oMarcaCLS.idmarca;
            using (var bd = new BDPasajeEntities())
            {
                Marca oMarca = bd.Marca.Where(p => p.IIDMARCA.Equals(idMarca)).First();
                oMarca.NOMBRE = oMarcaCLS.nombre;
                oMarca.DESCRIPCION = oMarcaCLS.descripcion;
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int id)
        {
            using (var bd = new BDPasajeEntities())
            {
                Marca oMarca = bd.Marca.Where(p => p.IIDMARCA.Equals(id)).First();
                oMarca.BHABILITADO = 0;
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}