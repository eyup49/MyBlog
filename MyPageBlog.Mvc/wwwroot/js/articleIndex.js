$(document).ready(function () {

    /*Datatable start here*/

    const dataTable= $('#articleTable').DataTable({
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
                    let url = window.location.href;
                    url = url.replace("/Index", "");
                    window.open(`${url}/Add`, "_self");
                }
            }
            ,
            {
                text: 'Yenile',
                className: 'btn btn-success rounded-pill',
                action: function (e, dt, node, config) {
                    $.ajax({
                        type: 'GET',
                        url: '/Admin/User/GetAllUsers/',
                        contentType: "application/json",
                        beforeSend: function () {
                            $('#userTable').hide();
                            $('.spinner-grow').show();
                        },
                        success: function (data) {
                            const userListDto = jQuery.parseJSON(data);
                            dataTable.clear();
                            if (userListDto.ResultStatus === 0) {                              
                                $.each(userListDto.Users.$values,
                                    function (index, user) {
                                        const newTableRaw= dataTable.row.add([
                                            user.Id,
                                            user.UserName,
                                            user.Email,
                                            user.PhoneNumber,

                                            `<img src="/img/${user.Picture}" alt="${user.UserName}" class="my-image-table"/>`,
                                            `<td>
                                                <button class="btn btn-outline-success  btn-sm btn-update" data-id="${user.Id}"><i class="fas fa-edit"></i> </button>
                                                <button class="btn btn-outline-danger  btn-sm btn-delete " data-id="${user.Id}"><i class="fas fa-trash-alt"></i></button>
                                             </td>`
                                        ]).node();
                                        const jqueryTableRow = $(newTableRaw);
                                        jqueryTableRow.attr('name', `${user.Id}`);

                                    });
                                dataTable.draw();
                              
                                $('.spinner-grow').hide();
                                $('#userTable').fadeIn(2000)
                            }
                            else {
                                toastr.error(`${userListDto.Message}`, 'İşlem Başarısız!');
                            }
                        },
                        error: function (err) {
                            $('.spinner-grow').hide();
                            $('#userTable').fadeIn(1000)
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
    /* Ajax Get / Getting the _UserAddPartial as Modal form starts from here */
    $(function () {
        const url = '/Admin/User/Add/';
        const modalPartialDiv = $('#modalPartial');
        $('#btnAdd').click(function () {
            $.get(url).done(function (data) {
                modalPartialDiv.html(data);
                modalPartialDiv.find(".modal").modal('show');
            });
        });
        /* Ajax Get / Getting the _UserAddPartial as Modal form ends  here */
        /* Ajax Post / Posting the FormData as UserAddDto  starts from here */
        modalPartialDiv.on('click', '#btnSave',
            function (event) {
                event.preventDefault();
                const form = $('#form-user-add');
                const actionUrl = form.attr('action');
                const dataToSend = new FormData(form.get(0));
                $.ajax({
                    url: actionUrl,
                    type: 'POST',
                    data: dataToSend,
                    processData: false,
                    contentType: false,
                    success: function (data) {
                        console.log(data);
                        const userAddAjaxModel = jQuery.parseJSON(data);
                        console.log(userAddAjaxModel);
                        const newFormBody = $('.modal-body', userAddAjaxModel.UserAddPartial);
                        modalPartialDiv.find('.modal-body').replaceWith(newFormBody);
                        const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                        if (isValid) {
                            modalPartialDiv.find('.modal').modal('hide');
                            const newTableRaw= dataTable.row.add([
                                userAddAjaxModel.UserDto.User.Id,
                                userAddAjaxModel.UserDto.User.UserName,
                                userAddAjaxModel.UserDto.User.Email,
                                userAddAjaxModel.UserDto.User.PhoneNumber,

                                `<img src="/img/${userAddAjaxModel.UserDto.User.Picture}" alt="${userAddAjaxModel.UserDto.User.UserName}" class="my-image-table"/>`,
                                `<td>
                                    <button class="btn btn-outline-success  btn-sm btn-update" data-id="${userAddAjaxModel.UserDto.User.Id}"><i class="fas fa-edit"></i> </button>
                                    <button class="btn btn-outline-danger  btn-sm btn-delete " data-id="${userAddAjaxModel.UserDto.User.Id}"><i class="fas fa-trash-alt"></i></button>
                                 </td>`
                            ]).node();
                            const jqueryTableRow = $(newTableRaw);
                            jqueryTableRow.attr('name', `${userAddAjaxModel.UserDto.User.Id}`);
                            dataTable.row(newTableRaw).draw();
                            toastr.success(`${userAddAjaxModel.UserDto.Message}`, 'Başarılı işlem!');

                        }
                        else {
                            let summaryText = "";
                            $('#validation-summary>ul>li').each(function () {
                                let text = $(this).text();
                                summaryText = `*${text}\n`;

                            });
                            toastr.warning(summaryText);
                        }
                    },
                    error: function( err) {
                    console.log(err);
                    }
                });
            });
    });

    //Silme alanı
    $(document).on('click', '.btn-delete', function (event) {
        event.preventDefault();
        const id = $(this).attr('data-id');
        const tableRow = $(`[name="${id}"]`);
        const userName = tableRow.find('td:eq(1)').text();
        Swal.fire({
            title: 'Silmek istediğinizden emin misiniz?',
            text: `${userName} adlı kategori silinecektir!`,
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
                    data: { userId: id },
                    url: '/Admin/User/Delete/")',
                    success: function (data) {

                        const userDto = jQuery.parseJSON(data);
                        if (userDto.ResultStatus === 0) {
                            Swal.fire(
                                'Silindi!',
                                `  başarıyla silinmiştir`,
                                'success'
                            );

                            dataTable.row(tableRow).remove().draw();
                        }
                        else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: `${userDto.Message}`,

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

    //Güncelleme kısmı
    $(function () {
        const url = '/Admin/User/Update/';
        const modalPartialDiv = $('#modalPartial');
        $(document).on('click', '.btn-update',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                $.get(url, { userId:id }).done(function (data) {
                    modalPartialDiv.html(data);
                    modalPartialDiv.find('.modal').modal('show');

                }).fail(function () {
                    toastr.error("bir hata oluştu");
                });
            });
        /*Ajax POST / Updating a User starts from here*/
        modalPartialDiv.on('click', '#btnUpdate', function (event) {
            event.preventDefault();
            const form = $('#form-user-update');
            const actionUrl = form.attr('action');
            const dataToSend = new FormData(form.get(0));
            $.ajax({
                url: actionUrl,
                type: 'POST',
                data: dataToSend,
                processData: false,
                contentType: false,
                success: function (data) {
                    console.log(data);
                    const userUpdateAjaxModel = jQuery.parseJSON(data);
                    console.log(userUpdateAjaxModel);
                    const id = userUpdateAjaxModel.UserDto.User.Id;
                    const tableRow = $(`[name="${id}"]`);
                    const newFormBody = $('.modal-body', userUpdateAjaxModel.UserUpdatePartial);
                    modalPartialDiv.find('.modal-body').replaceWith(newFormBody);
                    const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                    if (isValid) {
                        modalPartialDiv.find('.modal').modal('hide');

                        dataTable.row(tableRow).data([
                            userUpdateAjaxModel.UserDto.User.Id,
                            userUpdateAjaxModel.UserDto.User.UserName,
                            userUpdateAjaxModel.UserDto.User.Email,
                            userUpdateAjaxModel.UserDto.User.PhoneNumber,

                            `<img src="/img/${userUpdateAjaxModel.UserDto.User.Picture}" alt="${userUpdateAjaxModel.UserDto.User.UserName}" class="my-image-table" />`,
                            `<td>
                                    <button class="btn btn-outline-success  btn-sm btn-update" data-id="${userUpdateAjaxModel.UserDto.User.Id}"><i class="fas fa-edit"></i> </button>
                                    <button class="btn btn-outline-danger  btn-sm btn-delete " data-id="${userUpdateAjaxModel.UserDto.User.Id}"><i class="fas fa-trash-alt"></i></button>
                                 </td>`
                        ]);
                        tableRow.attr("name", `${id}`);
                        dataTable.row(tableRow).invalidate();

                        toastr.success(`${userUpdateAjaxModel.UserDto.Message}`, 'Başarılı işlem!');



                    }
                    else {
                        let summaryText = "";
                        $('#validation-summary>ul>li').each(function () {
                            let text = $(this).text();
                            summaryText = `*${text}\n`;

                        });
                        toastr.warning(summaryText);
                    }
                },
                error: function (error) {
                    console.log(error);
                }
            });

    });
    });
});