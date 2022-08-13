$(document).ready(function () {
    TestGrad.InitalizeComponent();
});
var TestGrad;
(function (TestGrad) {
    var Grid = new ESGrid();
    //var ShowData: HTMLButtonElement;
    var GenerateModels;
    var ConactServer;
    var DataSours;
    function InitalizeComponent() {
        //ShowData = document.getElementById('ShowData') as HTMLButtonElement
        GenerateModels = document.getElementById('GenerateModels');
        ConactServer = document.getElementById('ConactServer');
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
        //ShowData.onclick = ShowData_onclick;
        InitializeGridControl();
    }
    TestGrad.InitalizeComponent = InitalizeComponent;
    function ConactServer_onclick() {
        GetsqlData();
    }
    function GenerateModels_onclick() {
        GenerateMode();
    }
    function ShowData_onclick() {
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
        var _Data = JSON.stringify(modelSql);
        Ajax.CallAsync({
            url: Url.Action("ShowData", "GeneralSQL"),
            data: { RepP: _Data },
            success: function (d) {
                var result = d;
                debugger;
                var res = result;
                var Model = JSON.parse(res);
                Grid.ESG.LastCounter = 0;
                Grid.ESG.LastCounterAdd = 0;
                DisplayDataGridControl(Model, Grid);
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
    function GenerateMode() {
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
        var _Data = JSON.stringify(modelSql);
        Ajax.CallAsync({
            url: Url.Action("GenerateModelsTest", "GeneralSQL"),
            data: { RepP: _Data },
            success: function (d) {
                var result = d;
                debugger;
                var res = result;
                var Model = JSON.parse(res);
                Grid.Column = new Array();
                var properties = Object.getOwnPropertyNames(Model);
                for (var _i = 0, properties_1 = properties; _i < properties_1.length; _i++) {
                    var property = properties_1[_i];
                    var Colum = new Column();
                    Colum.Name = "" + property + "";
                    Colum.title = "" + property + "";
                    Grid.Column.push(Colum);
                }
                debugger;
                Model['StatusFlag'] = '';
                Grid.ESG.object = Model;
                Grid.ESG.LastCounter = 0;
                Grid.ESG.LastCounterAdd = 0;
                BindGridControl(Grid);
                setTimeout(function () { ShowData_onclick(); }, 200);
            }
        });
    }
    function InitializeGridControl() {
        Grid.ESG.NameTable = 'Grad1';
        Grid.ESG.OnfunctionSave = SaveNew;
        Grid.ESG.OnfunctionTotal = computeTotal;
        Grid.ESG.OnRowDoubleClicked = DoubleClicked;
        //DisplayDataGridControl(I_D_UOMDetails, Grid);
    }
    function SaveNew() {
        debugger;
        //alert(Grid.ESG.Model)
        console.log(Grid.ESG.Model);
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
        modelSql.Model = Grid.ESG.Model;
        var _Data = JSON.stringify(modelSql);
        Ajax.CallAsync({
            url: Url.Action("InsetData", "GeneralSQL"),
            data: { RepP: _Data },
            success: function (d) {
                var result = d;
                debugger;
                var res = result;
                var Model = JSON.parse(res);
                Grid.ESG.LastCounter = 0;
                Grid.ESG.LastCounterAdd = 0;
                DisplayDataGridControl(Model, Grid);
            }
        });
    }
    function computeTotal() {
        console.log(Grid.ESG.TotalModel);
    }
    function DoubleClicked() {
        alert(Grid.ESG.SelectedKey);
    }
})(TestGrad || (TestGrad = {}));
//# sourceMappingURL=TestGrad.js.map