Public Interface IManufacturerService
    ''' <summary>
    ''' 取得大氣廠商下拉式選單資料
    ''' </summary>
    ''' <returns></returns>
    Function GetGasVendorCmbItems() As List(Of SelectListItem)

    ''' <summary>
    ''' 取得廠商下拉式選單資料
    ''' </summary>
    ''' <returns></returns>
    Function GetVendorCmbItems() As List(Of SelectListItem)

    ''' <summary>
    ''' 取得大氣廠商外的廠商下拉選單資料
    ''' </summary>
    ''' <returns></returns>
    Function GetVendorCmbItemsWithoutGas() As List(Of SelectListItem)
End Interface
