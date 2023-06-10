/** 
 * 實作圖書管理系統
 * 
 * 1. 將搜尋的「圖書類別」套用 Kendo DropDownList
 * 2. 把 Kendo Grid 新增顯示「圖書類別」欄位
 * 3. 將圖書系統的「新增書籍」、「查詢書籍」、「刪除書籍」、「清除查詢資料」功能完成
 * 4. 新增書籍在變更「圖書類別」時，應對應變換圖片
 * 5. 將 Grid 的「書名」點擊後，跳出 Kendo Window 顯示單一本書的書籍明細
 * 
 * 加分題：新增書籍增加欄位驗證(使用 Kendo Validator)
 * 
 */

var bookDataFromLocalStorage = [];

$(function () {
    loadBookData();
    registerRegularComponent();

    //1. 將搜尋的「圖書類別」套用 Kendo DropDownList
    $("#book_class_q").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: classData,
        index: 0,
    });


    $("#book_detail_area").kendoWindow({
        width: "900px",
        title: "新增書籍",
        visible: false,
        modal: true,
        actions: [
            "Close"
        ],
        close: onBookWindowClose
    }).data("kendoWindow").center();

    $("#btn_add_book").click(function (e) {
        e.preventDefault();
        $("#btn-save").css("display", "");
        $("#book_detail_area").data("kendoWindow").title("新增書籍");
        $("#book_detail_area").data("kendoWindow").open();
    });

    // 查詢書籍按鈕
    $("#btn_query").click(function (e) {
        e.preventDefault();
        queryBook();
    });

    // 清除書籍按鈕      3. 
    $("#btn_clear").click(function (e) {
        e.preventDefault();
        document.getElementById('book_name_q').value = '';
        $("#book_class_q").data("kendoDropDownList").select(0);
        BookGrid_refresh();
        // ...
    });

    // 新增書籍按鈕
    $("#btn-save").click(function (e) {
        e.preventDefault();
        addBook();
    });

    $("#book_grid").kendoGrid({
        dataSource: {
            data: bookDataFromLocalStorage,
            schema: {
                model: {
                    id: "BookId",
                    fields: {
                        BookId: { type: "int" },
                        BookClassName: { type: "string" },
                        BookName: { type: "string" },
                        BookBoughtDate: { type: "string" }
                    }
                }
            },
            pageSize: 20,
        },
        height: 550,
        sortable: true,
        pageable: {
            input: true,
            numeric: false
        },
        columns: [
            { field: "BookId", title: "書籍編號", width: "10%" },
            {
                field: "BookName", title: "書名", width: "40%",
                template: "<a style='cursor:pointer; color:blue' onclick='showBookForDetail(event,#:BookId #)'>#: BookName #</a>"
            },
            { field: "BookClassName", title: "類別", width: "20%" },        //2. 把 Kendo Grid 新增顯示「圖書類別」欄位
            { field: "BookBoughtDate", title: "購書日期", width: "15%" },
            { command: { text: "刪除", click: deleteBook }, title: " ", width: "100px" }
        ]
    });
})

/**
 * 讀取書籍資料
 *
 */
function loadBookData() {
    bookDataFromLocalStorage = JSON.parse(localStorage.getItem("bookData"));
    if (bookDataFromLocalStorage == null) {
        bookDataFromLocalStorage = bookData;
        localStorage.setItem("bookData", JSON.stringify(bookDataFromLocalStorage));
    }
}

/**
 * 更換圖書類別   4. 
 *
 */
function onClassChange() {
    var value = document.getElementById("book_class_d").value;
    switch (value)
    {
        case "BK":
            $("#book_image_d").attr("src", "image/BK.jpg");
            break;
        case "DB":
            $("#book_image_d").attr("src", "image/DB.jpg");
            break;
        case "LG":
            $("#book_image_d").attr("src", "image/LG.jpg");
            break;
        case "LR":
            $("#book_image_d").attr("src", "image/LR.jpg");
            break;
        case "MG":
            $("#book_image_d").attr("src", "image/MG.jpg");
            break;
        case "MK":
            $("#book_image_d").attr("src", "image/MK.jpg");
            break;
        case "NW":
            $("#book_image_d").attr("src", "image/NW.jpg");
            break;
        case "OS":
            $("#book_image_d").attr("src", "image/OS.jpg");
            break;
        case "SC":
            $("#book_image_d").attr("src", "image/SC.jpg");
            break;
        case "SE":
            $("#book_image_d").attr("src", "image/optional.jpg");
            break;
        case "OT":
            $("#book_image_d").attr("src", "image/OT.jpg");
            break;
        case "TRCD":
            $("#book_image_d").attr("src", "image/TRCD.jpg");
            break;
        case "SECD":
            $("#book_image_d").attr("src", "image/SECD.jpg");
            break;
        default:
            $("#book_image_d").attr("src", "image/optional.jpg");
            break;
    }
}

/**
 * 關閉書籍視窗事件
 */
function onBookWindowClose() {
    // ...
}

/**
 * 新增書籍   3. 
 */
function addBook() {
    var book_class_d = $("#book_class_d").val()  //document.getElementById("book_class_d").value;
    var book_name_d = document.getElementById("book_name_d").value;
    var book_bought_date_d = document.getElementById("book_bought_date_d").value;
    var book_author_d = document.getElementById("book_author_d").value;
    var book_publisher_d = document.getElementById("book_publisher_d").value;
    var book_note_d = document.getElementById("book_note_d").value;
    var book_class_name = $("#book_class_d").data("kendoDropdwonList").text();
    //對應BookClassId 和 BookClassName
    /*
    for (var i = 0; i < classData.length;i++)
    {
        if (book_class_d == classData[i].value)
        {
            book_class_name = classData[i].text;
        }
    }
    */
    console.log(book_class_name);
    //新增資料到 bookDataFromLocalStorage
    bookDataFromLocalStorage.push(
        {
            "BookId": bookDataFromLocalStorage.length + 1,
            "BookName": book_name_d,
            "BookClassId": book_class_d,
            "BookClassName": book_class_name,
            "BookBoughtDate": book_bought_date_d,
            "BookStatusId": "A",
            "BookStatusName": "可以借出",
            "BookKeeperId": "",
            "BookKeeperCname": "",
            "BookKeeperEname": "",
            "BookAuthor": book_author_d,
            "BookPublisher": book_publisher_d,
            "BookNote": book_note_d
        });
    //新增資料到 bookData
    localStorage.setItem("bookData", JSON.stringify(bookDataFromLocalStorage));
    //reset input欄位
    $("#book_detail_area").data("kendoWindow").close();
    $("#book_class_d").data("kendoDropDownList").select(0);
    document.getElementById("book_name_d").value = '';
    $("#book_bought_date_d").data("kendoDatePicker").value(new Date());
    document.getElementById("book_author_d").value = '';
    document.getElementById("book_publisher_d").value = '';
    document.getElementById("book_note_d").value = '';
    $("#book_image_d").attr("src", "image/optional.jpg");
    BookGrid_refresh();
    // ...
}

/**
 * 搜尋書籍  3. 
 * 
 * Hint: 使用 Kendo Grid DataSource Filter
 * https://docs.telerik.com/kendo-ui/api/javascript/data/datasource/configuration/filter
 *
 */
function queryBook() {
    var grid = getBooGrid();
    var datasource = grid.dataSource;
    console.log(book_class_q.value);
    var book_class_name;
    for (var i = 0; i < classData.length; i++) {
        if (book_class_q.value == classData[i].value) {
            book_class_name = classData[i].text;
        }
    }
    console.log(book_name_q.value);
    if (book_name_q.value == '') {
        datasource.filter({ field: "BookClassName", operator: "eq", value: book_class_name });
    }
    else
    {
        datasource.filter(
            {
                logic: "and",
                filters: [{ field: "BookName", operator: "eq", value: book_name_q.value },
                { field: "BookClassName", operator: "eq", value: book_class_name }]
            });
    }
    grid.refresh();
    // ...
}

/**
 * 刪除書籍   3. 
 *
 * @param {*} e
 */
function deleteBook(e) {
    e.preventDefault();
    var grid = this;
    var row = $(e.currentTarget).closest("tr");
    var obj = grid.dataItem(row);
    for (var i = 0; i < bookDataFromLocalStorage.length; i++)
    {
        if (bookDataFromLocalStorage[i].BookId == obj.BookId)
        {
            bookDataFromLocalStorage.splice(i, 1);
            localStorage.setItem("bookData", JSON.stringify(bookDataFromLocalStorage));
            BookGrid_refresh();
            break;
        }
    }

    // ...
}

/**
 * 加分題: 顯示書籍明細
 *
 * @param {*} e
 * @param {*} bookId
 */
function showBookForDetail(e, bookId) {
    e.preventDefault();
    // ...
}

/**
 * 註冊控制項
 */
function registerRegularComponent() {

    $("#book_class_d").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: classData,
        optionLabel: "請選擇",
        index: 0,
        change: onClassChange
    });

    $("#book_bought_date_d").kendoDatePicker({
        format: "yyyy-MM-dd",
        value: new Date(),
        dateInput: true
    });
}

/**
 * 取得 Grid 元件
 *
 * @return {*} 
 */
function getBooGrid() {
    return $("#book_grid").data("kendoGrid");
}


function BookGrid_refresh()
{
    var grid = getBooGrid();
    var datasource = grid.dataSource;
    datasource.data(bookDataFromLocalStorage);
    datasource.filter({});
    grid.refresh();
}

