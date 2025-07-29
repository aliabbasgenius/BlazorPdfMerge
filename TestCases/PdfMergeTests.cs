using System.IO;
using System.Collections.Generic;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using Xunit;

public class PdfMergeTests
{
    // Helper: Create a PDF with a given number of pages in memory
    private MemoryStream CreatePdfWithPages(int pageCount)
    {
        var ms = new MemoryStream();
        using (var doc = new PdfDocument())
        {
            for (int i = 0; i < pageCount; i++)
            {
                doc.AddPage();
            }
            doc.Save(ms, false);
        }
        ms.Position = 0;
        return ms;
    }

    [Fact]
    public void MergePdfStreams_MergesPagesCorrectly()
    {
        // Arrange: Create two PDFs, one with 2 pages, one with 3 pages
        var pdf1 = CreatePdfWithPages(2);
        var pdf2 = CreatePdfWithPages(3);

        var pdfStreams = new List<Stream> { pdf1, pdf2 };

        // Act: Merge
        var mergedBytes = MergePdfWeb.Pages.Index.MergePdfStreams(pdfStreams);

        // Assert: Merged PDF should have 5 pages
        using var mergedStream = new MemoryStream(mergedBytes);
        using var mergedDoc = PdfReader.Open(mergedStream, PdfDocumentOpenMode.ReadOnly);
        Assert.Equal(5, mergedDoc.PageCount);
    }

    [Fact]
    public void MergePdfStreams_EmptyInput_ReturnsEmptyPdf()
    {
        // Arrange: No PDFs
        var pdfStreams = new List<Stream>();

        // Act
        var mergedBytes = MergePdfWeb.Pages.Index.MergePdfStreams(pdfStreams);

        // Assert: Merged PDF should have 0 pages
        using var mergedStream = new MemoryStream(mergedBytes);
        using var mergedDoc = PdfReader.Open(mergedStream, PdfDocumentOpenMode.ReadOnly);
        Assert.Equal(0, mergedDoc.PageCount);
    }

    // You can add more tests for edge cases as needed
}
