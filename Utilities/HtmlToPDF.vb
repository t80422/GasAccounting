Imports iText.Html2pdf
Imports iText.Html2pdf.Resolver.Font
Imports iText.Kernel.Pdf

Public Class HtmlToPDF
    ''' <summary>
    ''' 將HTML內容轉換為PDF文件並保存到指定路徑
    ''' </summary>
    ''' <param name="htmlContent">HTML內容</param>
    ''' <param name="pdfFilePath">PDF文件保存路徑</param>
    Public Sub ConvertHtmlToPdf(htmlContent As String, pdfFilePath As String)
        Using pdfWriter = New PdfWriter(pdfFilePath)
            Using pdfDocument = New PdfDocument(pdfWriter)
                Dim fontProvider As New DefaultFontProvider()
                fontProvider.AddFont("c:/windows/Fonts/MINGLIU_HKSCS.TTC")
                Dim converterProperties As New ConverterProperties()
                converterProperties.SetFontProvider(fontProvider)
                HtmlConverter.ConvertToPdf(htmlContent, pdfDocument, converterProperties)
            End Using
        End Using
    End Sub
End Class
