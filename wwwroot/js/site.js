// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// DataTables Türkçe dil desteği
if ($.fn.dataTable) {
    $.extend(true, $.fn.dataTable.defaults, {
        language: {
            url: "//cdn.datatables.net/plug-ins/1.11.5/i18n/tr.json"
        },
        pageLength: 10,
        lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "Tümü"]],
        responsive: true,
        stateSave: true
    });
}

// Bootstrap Tooltips
var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl);
});

// Form validation
(function () {
    'use strict';
    var forms = document.querySelectorAll('.needs-validation');
    Array.prototype.slice.call(forms).forEach(function (form) {
        form.addEventListener('submit', function (event) {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        }, false);
    });
})();

// Confirm dialog
function confirmDelete(message, callback) {
    if (confirm(message || 'Bu işlemi gerçekleştirmek istediğinizden emin misiniz?')) {
        callback();
    }
}

// AJAX error handler
$(document).ajaxError(function (event, jqXHR, ajaxSettings, thrownError) {
    if (jqXHR.status === 401) {
        window.location.href = '/Account/Login';
    } else if (jqXHR.status === 403) {
        alert('Bu işlem için yetkiniz bulunmamaktadır.');
    } else {
        alert('Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.');
    }
});

// Print function
function printElement(elementId) {
    var element = document.getElementById(elementId);
    var originalContents = document.body.innerHTML;
    var printContents = element.innerHTML;
    
    document.body.innerHTML = printContents;
    window.print();
    document.body.innerHTML = originalContents;
}

// Date formatting
function formatDate(date) {
    if (!date) return '';
    var d = new Date(date);
    return d.toLocaleDateString('tr-TR', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
}

// Input masks
if (typeof IMask !== 'undefined') {
    // Telefon maskesi
    document.querySelectorAll('input[type="tel"]').forEach(function(element) {
        IMask(element, {
            mask: '(000) 000-0000'
        });
    });

    // TC Kimlik No maskesi
    document.querySelectorAll('input[name="TCKimlikNo"]').forEach(function(element) {
        IMask(element, {
            mask: '00000000000'
        });
    });
}

// Responsive tables
document.querySelectorAll('.table-responsive').forEach(function(table) {
    var wrapper = document.createElement('div');
    wrapper.style.overflowX = 'auto';
    table.parentNode.insertBefore(wrapper, table);
    wrapper.appendChild(table);
});

// Date and time validation functions
function validateAppointmentDate(date) {
    const selectedDate = new Date(date);
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    if (selectedDate < today) {
        return { isValid: false, message: 'Geçmiş bir tarih seçemezsiniz.' };
    }

    const dayOfWeek = selectedDate.getDay();
    if (dayOfWeek === 0 || dayOfWeek === 6) {
        return { isValid: false, message: 'Hafta sonu randevu alınamaz.' };
    }

    return { isValid: true };
}

function validateAppointmentTime(time) {
    const [hours, minutes] = time.split(':').map(Number);
    const timeInMinutes = hours * 60 + minutes;

    if (timeInMinutes < 9 * 60 || timeInMinutes > 17 * 60) {
        return { isValid: false, message: 'Randevular 09:00 - 17:00 arasında alınabilir.' };
    }

    if (minutes % 30 !== 0) {
        return { isValid: false, message: 'Randevular 30 dakikalık aralıklarla alınabilir.' };
    }

    return { isValid: true };
}

// DataTables Turkish language
const dataTablesTurkish = {
    "emptyTable": "Tabloda herhangi bir veri mevcut değil",
    "info": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
    "infoEmpty": "Kayıt yok",
    "infoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
    "infoThousands": ".",
    "lengthMenu": "Sayfada _MENU_ kayıt göster",
    "loadingRecords": "Yükleniyor...",
    "processing": "İşleniyor...",
    "search": "Ara:",
    "zeroRecords": "Eşleşen kayıt bulunamadı",
    "paginate": {
        "first": "İlk",
        "last": "Son",
        "next": "Sonraki",
        "previous": "Önceki"
    },
    "aria": {
        "sortAscending": ": artan sütun sıralamasını aktifleştir",
        "sortDescending": ": azalan sütun sıralamasını aktifleştir"
    },
    "select": {
        "rows": {
            "_": "%d kayıt seçildi",
            "1": "1 kayıt seçildi"
        }
    }
};

// Initialize DataTables with Turkish language
$(document).ready(function () {
    $('.datatable').DataTable({
        language: dataTablesTurkish,
        order: [[0, 'desc']]
    });
});
