$(document).ready(function () {
    TestGrad.InitalizeComponent();
});
var TestGrad;
(function (TestGrad) {
    var sys = new SystemTools();
    var SysSession = GetSystemSession(Modules.Quotation);
    var I_D_UOMDetails = new Array();
    var compcode; //SharedSession.CurrentEnvironment.CompCode;
    var BranchCode; //SharedSession.CurrentEnvironment.CompCode; 
    var Grid = new ESGrid();
    var SqlEn = new SqlEnt();
    var GenerateModels;
    var ConactServer;
    var ModelArea;
    var DataSours;
    function InitalizeComponent() {
        compcode = Number(SysSession.CurrentEnvironment.CompCode);
        BranchCode = Number(SysSession.CurrentEnvironment.BranchCode);
        GenerateModels = document.getElementById('GenerateModels');
        ConactServer = document.getElementById('ConactServer');
        ModelArea = document.getElementById('ModelArea');
        DataSours = document.getElementById('DataSours');
        //Ajax.Callsync({
        //    type: "Get",
        //    url: sys.apiUrl("SlsTrSales", "GetAllUOM"),
        //    success: (d) => {
        //        let result = d as BaseResponse;
        //        if (result.IsSuccess) {
        //            I_D_UOMDetails = result.Response as Array<I_D_UOM>;
        //        }
        //    }
        //});
        //InitializeGridControl(); 
        ConactServer.onclick = ConactServer_onclick;
        GenerateModels.onclick = GenerateModels_onclick;
    }
    TestGrad.InitalizeComponent = InitalizeComponent;
    function ConactServer_onclick() {
        GetsqlData();
    }
    function GenerateModels_onclick() {
        GenerateMode();
    }
    function GenerateMode() {
        //let rp: Array<SqlTables> = new Array<SqlTables>()
        //let SqlEn: SqlEnt = new SqlEnt();
        var model = new SqlTables();
        var modelSql = new ModelSql();
        var rp = new SqlEnt();
        rp.Database = $('#Database').val();
        rp.Server = $('#Server').val();
        rp.Password = $('#Password').val();
        rp.User = $('#User').val();
        model.name = $("#DataSours option:selected").text();
        model.object_id = $('#DataSours').val();
        modelSql.sqlTables = model;
        modelSql.sqlEnt = rp;
        //rp.push(model);
        console.log(modelSql);
        var _Data = JSON.stringify(modelSql);
        Ajax.CallAsync({
            url: Url.Action("GenerateModelsTest", "GeneralSQL"),
            data: { RepP: _Data },
            success: function (d) {
                var result = d;
                debugger;
                var res = result;
                var xx = JSON.parse(res);
                ModelArea.value = xx;
                //DocumentActions.FillCombowithdefult(result, DataSours, 'object_id', 'name', "Select Data Sours");
            }
        });
    }
    function GetsqlData() {
        var rp = new SqlEnt();
        rp.Database = $('#Database').val();
        rp.Server = $('#Server').val();
        rp.Password = $('#Password').val();
        rp.User = $('#User').val();
        Ajax.CallAsync({
            url: Url.Action("CounactData", "GeneralSQL"),
            data: rp,
            success: function (d) {
                var result = d;
                debugger;
                var res = result;
                DocumentActions.FillCombowithdefult(result, DataSours, 'object_id', 'name', "Select Data Sours");
            }
        });
    }
    function InitializeGridControl() {
        var _this = this;
        Grid.ESG.NameTable = 'Grad1';
        Grid.ESG.PrimaryKey = 'UomID';
        Grid.ESG.Right = true;
        Grid.ESG.Edit = true;
        Grid.ESG.Add = true;
        Grid.ESG.DeleteRow = true;
        Grid.ESG.CopyRow = true;
        Grid.ESG.Back = true;
        Grid.ESG.Save = true;
        Grid.ESG.OnfunctionSave = SaveNew;
        Grid.ESG.OnfunctionTotal = computeTotal;
        Grid.ESG.OnRowDoubleClicked = DoubleClicked;
        Grid.ESG.object = new I_D_UOM();
        Grid.Column = [
            { title: "ID", Name: "UomID", value: "0", Type: "text", style: "width: 10%", Edit: false, visible: false, Validation: Valid.Set(false), ColumnType: ControlType.Input() },
            { title: "الرقم", Name: "UomCode", value: "0", Type: "text", style: "width: 30%", Edit: true, visible: true, Validation: Valid.Set(true), ColumnType: ControlType.Dropdown(I_D_UOMDetails, 'DescA', function () { }, function () { }, function () { console.log(_this); }) },
            { title: "الاسم", Name: "DescA", value: "0", Type: "text", style: "width: 10%", Edit: true, visible: true, Validation: Valid.Set(false), ColumnType: ControlType.Input(function () { }, function () { }, function () { }) },
            { title: "العمر", Name: "DescE", value: "1", Type: "text", style: "width: 10%", Edit: true, visible: true, Validation: Valid.Set(false), ColumnType: ControlType.Input(function () { }, function () { }, function () { console.log(_this); }) },
            { title: "رقم التيلفون", Name: "CompCode", value: "0", Type: "text", style: "width: 10%", Edit: true, visible: true, Validation: Valid.Set(false), ColumnType: ControlType.Input(function () { }, function () { }, function () { console.log(_this); }) },
            { title: "رقم البطاقه", Name: "Remarks", value: "BUT", Type: "text", style: "width: 10%", Edit: true, visible: true, Validation: Valid.Set(false), ColumnType: ControlType.Input(function () { }, function () { }, function () { }) },
            { title: "النوع", Name: "CreatedAt", value: "0", Type: "text", style: "width: 10%", Edit: true, visible: true, Validation: Valid.Set(false), ColumnType: ControlType.Input(function () { }, function () { ('CreatedBy').Set_Val(('CreatedAt').Get_Val(Grid), Grid); }, function () { console.log(_this.propone); }) },
            { title: "الملاحظات", Name: "CreatedBy", value: "0", Type: "text", style: "width: 10%", Edit: true, visible: true, Validation: Valid.Set(true, 'مينفع تساوي (100)', ['='], '100'), ColumnType: ControlType.Input(function () { }, function () { }, function () { console.log(_this); }) },
            { title: "رصيد", Name: "Cheack", value: "0", Type: "text", style: "width: 10%", Edit: true, visible: true, Validation: Valid.Set(false), ColumnType: ControlType.checkbox(function () { alert(('Cheack').Get_Cheak(Grid)); }, function () { }, function () { }) },
        ];
        BindGridControl(Grid);
        DisplayDataGridControl(I_D_UOMDetails, Grid);
    }
    function SaveNew() {
        debugger;
        alert(Grid.ESG.Model);
        console.log(Grid.ESG.Model);
    }
    function computeTotal() {
        console.log(Grid.ESG.TotalModel);
    }
    function DoubleClicked() {
        alert(Grid.ESG.SelectedKey);
    }
})(TestGrad || (TestGrad = {}));
//# sourceMappingURL=TestGrad.js.map