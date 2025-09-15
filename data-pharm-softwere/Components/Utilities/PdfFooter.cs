using System;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace data_pharm_softwere.Components.Utilities
{
    public class PdfFooter : PdfPageEventHelper
    {
        private PdfContentByte cb;
        private PdfTemplate footerTemplate;
        private BaseFont bf;
        private DateTime printTime;

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            printTime = DateTime.Now;
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb = writer.DirectContent;
            footerTemplate = cb.CreateTemplate(50, 50);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            int pageN = writer.PageNumber;
            string textLeft = printTime.ToString("dd MMM, yyyy h:mmtt");
            string textRight = "Page " + pageN + " of ";

            float lenRight = bf.GetWidthPoint(textRight, 8);
            Rectangle pageSize = document.PageSize;

            // Left side (date/time)
            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetBottom(20));
            cb.ShowText(textLeft);
            cb.EndText();

            // Right side (Page X of )
            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.SetTextMatrix(pageSize.GetRight(100), pageSize.GetBottom(20));
            cb.ShowText(textRight);
            cb.EndText();

            // Placeholder for total pages
            cb.AddTemplate(footerTemplate, pageSize.GetRight(100) + lenRight, pageSize.GetBottom(20));
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            footerTemplate.BeginText();
            footerTemplate.SetFontAndSize(bf, 8);
            footerTemplate.ShowText("" + writer.PageNumber);
            footerTemplate.EndText();
        }
    }
}