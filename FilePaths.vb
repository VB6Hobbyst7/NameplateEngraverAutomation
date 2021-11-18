Module FilePaths
    Public VrTemplatesPath As String = "I:\Shared\CroppedNameplates2\RegulatorTemplates\"
    Public acadLspPath As String = "C:\Program Files\Autodesk\AutoCAD 2020\Support\" '"C:\Program Files\Autodesk\AutoCAD 2016\Support\"
    Public batPath As String = "I:\Shared\CroppedNameplates2\RegulatorTemplates\RunAcad.bat"
    Public croppedNameplatesPath As String = "I:\Shared\CroppedNameplates2\"
    Public dwg2dxfbat As String

    'EMK 10/2021
    'Added to avoid the read only mode pop up when opening a file directly from the regulator templates folder...
    Public copiedVRDWG As String = "I:\Shared\CroppedNameplates2\RegulatorTemplates\RegulatorDWGs"
    Public acadPath As String = "C:\Program Files\Autodesk\AutoCAD 2020\acad.exe"
End Module
