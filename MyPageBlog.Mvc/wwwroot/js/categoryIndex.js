$(document).ready(function () {

    /*Datatable start here*/

    $('#categoriesTable').DataTable({
        dom:
            "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
            {
                text: 'Ekle',
                attr: {
                    id: "btnAdd",
                },
                className: 'btn btn-warning rounded-pill mr-1',
                action: function (e, dt, node, config) {

                }
            }
            ,
            {
                text: 'Yenile',
                className: 'btn btn-success rounded-pill',
                action: function (e, dt, node, config) {
                    $.ajax({
                        type: 'GET',
                        url: '/Admin/Category/GetAllCategories/',
                        contentType: "application/json",
                        beforeSend: function () {
                            $('#categoriesTable').hide();
                            $('.spinner-grow').show();
                        },
                        success: function (data) {
                            const categoryListDto = jQuery.parseJSON(data);
                            if (categoryListDto.ResultStatus === 0) {
                                let tableBody = "";
                                $.each(categoryListDto.Categories.$values, function (index, category) {
                                    tableBody +=
                                        ` <tr>
                                                        <td>${category.Id}</td>
                                                        <td>${category.Name}</td>
                                                        <td>${category.Description}</td>
                                                        <td>${convertFirstLetterToUpperCase(category.IsActive.toString())}</td>
                                                        <td>${convertFirstLetterToUpperCase(category.IsDeleted.toString())}</td>
                                                        <td>${category.Note}</td>
                                                        <td>${convertToShortDate(category.CreatedDate)}</td>
                                                        <td>${category.CreatedByName}</td>
                                                        <td>${convertToShortDate(category.ModifiedDate)}</td>
                                                        <td>${category.ModifiedByName}</td>
                                                        <td>
                                                            <button class="btn btn-outline-success btn-sm btn-update " data-id="${category.Id}"><i class="fas fa-edit"></i></button>
                                                            <button class="btn btn-outline-danger btn-sm  btn-delete " data-id="${category.Id}"><i class="fas fa-trash-alt"></i></button>
                                                        </td>

                                                    </tr>
                                                    `;

                                });
                                $('#categoriesTable>tbody').replaceWith(tableBody);
                                $('.spinner-grow').hide();
                                $('#categoriesTable').fadeIn(2000)
                            }
                            else {
                                toastr.error(`${categoryListDto.Message}`, 'İşlem Başarısız!');
                            }
                        },
                        error: function (err) {
                            $('.spinner-grow').hide();
                            $('#categoriesTable').fadeIn(1000)
                            toastr.error(`${err.responseText}`, 'Hata!');

                        }
                    })
                }
            }
        ],
        language: {
            "sDecimal": ",",
            "sEmptyTable": "Tabloda herhangi bir veri mevcut değil",
            "sInfo": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
            "sInfoEmpty": "Kayıt yok",
            "sInfoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "Sayfada _MENU_ kayıt göster",
            "sLoadingRecords": "Yükleniyor...",
            "sProcessing": "İşleniyor...",
            "sSearch": "Ara:",
            "sZeroRecords": "Eşleşen kayıt bulunamadı",
            "oPaginate": {
                "sFirst": "İlk",
                "sLast": "Son",
                "sNext": "Sonraki",
                "sPrevious": "Önceki"
            },
            "oAria": {
                "sSortAscending": ": artan sütun sıralamasını aktifleştir",
                "sSortDescending": ": azalan sütun sıralamasını aktifleştir"
            },
            "select": {
                "rows": {
                    "_": "%d kayıt seçildi",
                    "0": "",
                    "1": "1 kayıt seçildi"
                }
            }
        }
    });
    /* datatable ends here */
    /* Ajax Get / Getting the_CategoryAddPartial as Modal form starts from here */
    $(function () {
        const url = '/Admin/Category/Add/';
        const modalPartialDiv = $('#modalPartial');
        $('#btnAdd').click(function () {
            $.get(url).done(function (data) {
                modalPartialDiv.html(data);
                modalPartialDiv.find(".modal").modal('show');
            });
        });
        /* Ajax Get / Getting the_CategoryAddPartial as Modal form ends  here */
        /* Ajax Post / Posting the FormData as CategoryAddDto  starts from here */
        modalPartialDiv.on('click', '#btnSave',
            function (event) {
                event.preventDefault();
                const form = $('#form-category-add');
                const actionUrl = form.attr('action');
                const dataToSend = form.serialize();
                $.post(actionUrl, dataToSend).done(function (data) {
                    console.log(data);
                    const categoryAddAjaxModel = jQuery.parseJSON(data);
                    console.log(categoryAddAjaxModel);
                    const newFormBody = $('.modal-body', categoryAddAjaxModel.CategoryAddPartial);
                    modalPartialDiv.find('.modal-body').replaceWith(newFormBody);
                    const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                    if (isValid) {
                        modalPartialDiv.find('.modal').modal('hide');
                        const newTableRow =
                            `  <tr name="${categoryUpdateAjaxModel.CategoryDto.Category.Id}">
                                            <td>${categoryAddAjaxModel.CategoryDto.Category.Id}</td>
                                            <td>${categoryAddAjaxModel.CategoryDto.Category.Name}</td>
                                            <td>${categoryAddAjaxModel.CategoryDto.Category.Description}</td>
                                            <td>${convertFirstLetterToUpperCase(categoryAddAjaxModel.CategoryDto.Category.IsActive.toString())}</td>
                                            <td>${convertFirstLetterToUpperCase(categoryAddAjaxModel.CategoryDto.Category.IsDeleted.toString())}</td>
                                            <td>${categoryAddAjaxModel.CategoryDto.Category.Note}</td>
                                            <td>${convertToShortDate(categoryAddAjaxModel.CategoryDto.Category.CreatedDate)}</td>
                                            <td>${categoryAddAjaxModel.CategoryDto.Category.CreatedByName}</td>
                                            <td>${convertToShortDate(categoryAddAjaxModel.CategoryDto.Category.ModifiedDate)}</td>
                                            <td>${categoryAddAjaxModel.CategoryDto.Category.ModifiedByName}</td>
                                            <td>
                                                <button class="btn btn-outline-success  btn-sm btn-update" data-id="${categoryAddAjaxModel.CategoryDto.Category.Id}"><i class="fas fa-edit"></i> </button>
                                                <button class="btn btn-outline-danger  btn-sm btn-delete " data-id="${categoryAddAjaxModel.CategoryDto.Category.Id}"><i class="fas fa-trash-alt"></i></button>
                                            </td>

                                        </tr>`;

                        const newTableRowObject = $(newTableRow);
                        newTableRowObject.hide();
                        $('#cateforiesTable').append(newTableRowObject);
                        newTableRowObject.fadeIn(3500);
                        toastr.success(`${categoryAddAjaxModel.CategoryDto.Message}`, 'Başarılı işlem!');

                    }
                    else {
                        let summaryText = "";
                        $('#validation-summary>ul>li').each(function () {
                            let text = $(this).text();
                            summaryText = `*${text}\n`;

                        });
                        toastr.warning(summaryText);
                    }
                });
            });
    });
    $(document).on('click', '.btn-delete', function (event) {
        event.preventDefault();
        const id = $(this).attr('data-id');
        const tableRow = $(`[name="${id}"]`);
        const categoryName = tableRow.find('td:eq(1)').text();
        Swal.fire({
            title: 'Silmek istediğinizden emin misiniz?',
            text: `${categoryName} adlı kategori silinecektir!`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Evet, silmek İstiyorum!',
            cancelButtonText: 'Hayır , silmek istemiyorum'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    data: { categoryId: id },
                    url: '/Admin/Category/Delete/")',
                    success: function (data) {

                        const categoryDto = jQuery.parseJSON(data);
                        if (categoryDto.ResultStatus === 0) {
                            Swal.fire(
                                'Silindi!',
                                ` kategori başarıyla silinmiştir`,
                                'success'
                            );

                            tableRow.fadeOut(3500);
                        }
                        else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: `${categoryDto.Message}`,

                            });
                        }
                    },
                    error: function (err) {
                        console.log(err);
                        toastr.error(`${err.responseText}`, "Hata!");
                    }
                })
            }
        });
    });
    $(function () {
        const url = '/Admin/Category/Update/';
        const modalPartialDiv = $('#modalPartial');
        $(document).on('click', '.btn-update',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                $.get(url, { categoryId:id }).done(function (data) {
                    modalPartialDiv.html(data);
                    modalPartialDiv.find('.modal').modal('show');

                }).fail(function () {
                    toastr.error("bir hata oluştu");
                });
            });
        /*Ajax POST / Updating a Category starts from here*/
        modalPartialDiv.on('click', '#btnUpdate', function (event) {
            event.preventDefault();
            const form = $('#form-category-update');
            const actionUrl = form.attr('action');
            const dataToSend = form.serialize();
            $.post(actionUrl, dataToSend).done(function (data) {
                console.log(data);
                const categoryUpdateAjaxModel = jQuery.parseJSON(data);
                console.log(categoryUpdateAjaxModel);
                const newFormBody = $('.modal-body', categoryUpdateAjaxModel.CategoryUpdatePartial);
                modalPartialDiv.find('.modal-body').replaceWith(newFormBody);
                const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                if (isValid) {
                    modalPartialDiv.find('.modal').modal('hide');
                    const newTableRow =
                        ` <tr name="${categoryUpdateAjaxModel.CategoryDto.Category.Id}">
                                            <td>${categoryUpdateAjaxModel.CategoryDto.Category.Id}</td>
                                            <td>${categoryUpdateAjaxModel.CategoryDto.Category.Name}</td>
                                            <td>${categoryUpdateAjaxModel.CategoryDto.Category.Description}</td>
                                            <td>${convertFirstLetterToUpperCase(categoryUpdateAjaxModel.CategoryDto.Category.IsActive.toString())}</td>
                                            <td>${convertFirstLetterToUpperCase(categoryUpdateAjaxModel.CategoryDto.Category.IsDeleted.toString())}</td>
                                            <td>${categoryUpdateAjaxModel.CategoryDto.Category.Note}</td>
                                            <td>${convertToShortDate(categoryUpdateAjaxModel.CategoryDto.Category.CreatedDate)}</td>
                                            <td>${categoryUpdateAjaxModel.CategoryDto.Category.CreatedByName}</td>
                                            <td>${convertToShortDate(categoryUpdateAjaxModel.CategoryDto.Category.ModifiedDate)}</td>
                                            <td>${categoryUpdateAjaxModel.CategoryDto.Category.ModifiedByName}</td>
                                            <td>
                                                <button class="btn btn-outline-success  btn-sm btn-update" data-id="${categoryUpdateAjaxModel.CategoryDto.Category.Id}"><i class="fas fa-edit"></i> </button>
                                                <button class="btn btn-outline-danger  btn-sm btn-delete " data-id="${categoryUpdateAjaxModel.CategoryDto.Category.Id}"><i class="fas fa-trash-alt"></i></button>
                                            </td>

                                        </tr>`;

                    const newTableRowObject = $(newTableRow);
                    const categoryTableRow = $(`[name="${categoryUpdateAjaxModel.CategoryDto.Category.Id}"]`);
                    newTableRowObject.hide();
                    categoryTableRow.replaceWith(newTableRowObject);
                    newTableRowObject.fadeIn(3500);
                    toastr.success(`${categoryUpdateAjaxModel.CategoryDto.Message}`, 'Başarılı işlem!');

                    

                }
                else {
                    let summaryText = "";
                    $('#validation-summary>ul>li').each(function () {
                        let text = $(this).text();
                        summaryText = `*${text}\n`;

                    });
                    toastr.warning(summaryText);
                }
            });

    });
    });
});