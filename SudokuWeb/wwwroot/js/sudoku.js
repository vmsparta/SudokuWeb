$(document).ready(function () {

    setDifficulty();
    loadTable(false, $('.difficulty:radio:checked').val());

    $("body").on("click", ".sudoku-editable-cell", function (e) {
        $(".sudoku-selected-cell").removeClass("sudoku-selected-cell");
        $(this).addClass("sudoku-selected-cell");

    });

    $("body").on("click", ".sudoku-num-selector td", function (e) {
        var val = $(this).text();
        var col = $(".sudoku-selected-cell").index();
        var row = $(".sudoku-selected-cell").closest('tr').index();

        saveCellValue(row, col, val);
    });

    $("#winImage").click(function () {
        $("#winImage").fadeOut("slow");
    });

    $("#btnNewGame").click(function () {
        loadTable(true, $('.difficulty:radio:checked').val());
    });

    $(document).on('change', '.difficulty', function (event) {
        $.cookie('difficulty', $(this).val());
    });

    $("#btnHint").click(function () {
        $.ajax({
            type: "GET",
            url: '/Game/GetHint',
            success: function (cell) {
                var td = $('.sudoku-table tr').eq(cell.rowIndex).find('td').eq(cell.columnIndex);
                td.text(cell.value);
                $(".sudoku-selected-cell").removeClass("sudoku-selected-cell");
                td.addClass("sudoku-selected-cell");
                saveCellValue(cell.rowIndex, cell.columnIndex, cell.value);
            }
        });
    });

    function popoverActivate() {
        $(".sudoku-editable-cell").popover({
            content: "<table class='sudoku-num-selector'>" +
                    "<tr>" +
                        "<td>1</td><td>2</td><td>3</td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td>4</td><td>5</td><td>6</td>" +
                    "</tr>"+
                    "<tr>" +
                        "<td>7</td> <td>8</td> <td>9</td></tr>" +
                    "<tr>" +
                        "<td class='sudoku-invisible-cell'></td><td>&nbsp;</td><td class='sudoku-invisible-cell'></td>" +
                    "</tr>" +
                "</table>",
            html: true,
            placement: "auto",
            trigger: "focus",
            container: ".sudoku-table"
        });
    }

    function loadTable(newGame, numCount) {
        $.ajax({
            url: '/Game/LoadTable',
            dataType: 'html',
            data: { createNewGame: newGame, visibleNumCount: numCount},
            success: function (data) {
                $('.sudoku-container').html(data);
                popoverActivate();
            }
        });
    }

    function setDifficulty() {
        if ($.cookie('difficulty')) {
            $('.difficulty:radio[value="' + $.cookie('difficulty') + '"]').attr('checked', 'checked');
        }
    }

    function saveCellValue(row, col, val) {
        var data = {
            rowIndex: row,
            columnIndex: col,
            value: val
        };

        $.ajax({
            type: "POST",
            url: "/Game/SaveCellValue",
            data: data,
            success: function (result) {
                $(".sudoku-selected-cell").first().text(val);
                if (result.isErrorValue)
                    $(".sudoku-selected-cell").addClass("sudoku-error-cell");
                else
                    $(".sudoku-selected-cell").removeClass("sudoku-error-cell");

                if (result.isLevelCompleted) {
                    $(".sudoku-editable-cell").popover('disable');
                    $("#winImage").fadeIn("slow");
                }
                $(".sudoku-selected-cell").removeClass("sudoku-selected-cell");
            },
            failure: function (response) {
                alert(response);
            }
        });
    }

});

