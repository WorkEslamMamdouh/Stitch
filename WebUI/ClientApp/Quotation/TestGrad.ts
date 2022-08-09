
$(document).ready(() => {
    TestGrad.InitalizeComponent();
})

namespace TestGrad {

    var sys: SystemTools = new SystemTools();
    var SysSession: SystemSession = GetSystemSession(Modules.Quotation);
    var I_D_UOMDetails: Array<I_D_UOM> = new Array<I_D_UOM>();
    var compcode: number;//SharedSession.CurrentEnvironment.CompCode;
    var BranchCode: number;//SharedSession.CurrentEnvironment.CompCode; 
    var Grid: ESGrid = new ESGrid();
    var SqlEn: SqlEnt = new SqlEnt();

    var GenerateModels: HTMLButtonElement;
    var ConactServer: HTMLButtonElement;
    var ModelArea: HTMLTextAreaElement;
    var DataSours: HTMLSelectElement;

    export function InitalizeComponent() {

        compcode = Number(SysSession.CurrentEnvironment.CompCode);
        BranchCode = Number(SysSession.CurrentEnvironment.BranchCode);

        GenerateModels = document.getElementById('GenerateModels') as HTMLButtonElement
        ConactServer = document.getElementById('ConactServer') as HTMLButtonElement
        ModelArea = document.getElementById('ModelArea') as HTMLTextAreaElement
        DataSours = document.getElementById('DataSours') as HTMLSelectElement



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

        InitializeGridControl();
    }


    function ConactServer_onclick() {

        GetsqlData();

    }

    function GenerateModels_onclick() {
        GenerateMode();
    }


    function GetsqlData() {

        let rp: SqlEnt = new SqlEnt();

        rp.Database = $('#Database').val();
        rp.Server = $('#Server').val();
        rp.Password = $('#Password').val();
        rp.User = $('#User').val();

        Ajax.CallAsync({
            url: Url.Action("CounactData", "GeneralSQL"),
            data: rp,
            success: (d) => {
                let result = d
                debugger
                let res = result as SqlTables;


                DocumentActions.FillCombowithdefult(result, DataSours, 'object_id', 'name', "Select Data Sours");


            }
        })

    }



    function GenerateMode() {
        $('#Grad1').html('');
        //let rp: Array<SqlTables> = new Array<SqlTables>()
        //let SqlEn: SqlEnt = new SqlEnt();
        let model: SqlTables = new SqlTables();
        let modelSql: ModelSql = new ModelSql();

        let rp: SqlEnt = new SqlEnt();

        rp.Database = $('#Database').val();
        rp.Server = $('#Server').val();
        rp.Password = $('#Password').val();
        rp.User = $('#User').val();

        model.name = $("#DataSours option:selected").text();
        model.object_id = $('#DataSours').val();


        modelSql.sqlTables = model;
        modelSql.sqlEnt = rp;

        //rp.push(model);

        console.log(modelSql)
        let _Data: string = JSON.stringify(modelSql);

        Ajax.CallAsync({
            url: Url.Action("GenerateModelsTest", "GeneralSQL"),
            data: { RepP: _Data },
            success: (d) => {
                let result = d
                debugger
                let res = result;

                var Model: any = JSON.parse(res);



                Grid.Column = new Array<Column>();
              


                let properties = Object.getOwnPropertyNames(Model);
                for (var property of properties) {

                    let Colum: Column = new Column();
                    Colum.Name = "" + property+"";
                    Colum.title = "" + property+"";
                    Grid.Column.push(Colum);

                }
                debugger
                Model['StatusFlag'] = '';
                Grid.ESG.object = Model;

                BindGridControl(Grid);

                //ModelArea.value = xx;

                //DocumentActions.FillCombowithdefult(result, DataSours, 'object_id', 'name', "Select Data Sours");


            }
        })

    }











    function InitializeGridControl() {

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
        //Grid.ESG.object = new I_D_UOM();


        ////Grid.ESG.object = new I_D_UOM();
        ////Grid.Column = new Array<Column>();
        ////for (var i = 0; i < length; i++) {

        ////    let Colum: Column = new Column();
        ////    Colum.Name = "UomID";
        ////    Colum.title = "UomID";
        ////    Grid.Column.push(Colum);

        ////}
        ////BindGridControl(Grid);

        //Grid.Column = [
        //    { title: "ID", Name: "UomID" },
        //    { title: "الرقم", Name: "UomCode" },
        //    { title: "الاسم", Name: "DescA", value: "0", Type: "text", style: "width: 10%", Edit: true, visible: true, Validation: Valid.Set(false), ColumnType: ControlType.Input(() => { }, () => { }, () => { }) },
        //    { title: "العمر", Name: "DescE", value: "1", Type: "text", style: "width: 10%", Edit: true, visible: true, Validation: Valid.Set(false), ColumnType: ControlType.Input(() => { }, () => { }, () => { console.log(this) }) },
        //    { title: "رقم التيلفون", Name: "CompCode", value: "0", Type: "text", style: "width: 10%", Edit: true, visible: true, Validation: Valid.Set(false), ColumnType: ControlType.Input(() => { }, () => { }, () => { console.log(this) }) },
        //    { title: "رقم البطاقه", Name: "Remarks", value: "BUT", Type: "text", style: "width: 10%", Edit: true, visible: true, Validation: Valid.Set(false), ColumnType: ControlType.Input(() => { }, () => { }, () => { }) },
        //    { title: "النوع", Name: "CreatedAt", value: "0", Type: "text", style: "width: 10%", Edit: true, visible: true, Validation: Valid.Set(false), ColumnType: ControlType.Input(() => { }, () => { ('CreatedBy').Set_Val(('CreatedAt').Get_Val(Grid), Grid) }, () => { console.log(this.propone) }) },
        //    { title: "الملاحظات", Name: "CreatedBy", value: "0", Type: "text", style: "width: 10%", Edit: true, visible: true, Validation: Valid.Set(true, 'مينفع تساوي (100)', ['='], '100'), ColumnType: ControlType.Input(() => { }, () => { }, () => { console.log(this) }) },
        //    { title: "رصيد", Name: "Cheack", value: "0", Type: "text", style: "width: 10%", Edit: true, visible: true, Validation: Valid.Set(false), ColumnType: ControlType.checkbox(() => { alert(('Cheack').Get_Cheak(Grid)) }, () => { }, () => { }) },
        //]

        //BindGridControl(Grid);
        //DisplayDataGridControl(I_D_UOMDetails, Grid);
    }
    function SaveNew() {
        debugger
        alert(Grid.ESG.Model)

        console.log(Grid.ESG.Model)
    }
    function computeTotal() {
        console.log(Grid.ESG.TotalModel);
    }
    function DoubleClicked() {
        alert(Grid.ESG.SelectedKey);
    }

}












