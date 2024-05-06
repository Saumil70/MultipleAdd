$(document).ready(function () {
    showUser();
    $('#country').attr('disable', true);
    $('.state').attr('disable', true);
    $('.city').attr('disable', true);
 

})


function showUser() {
    $.ajax({
        url: '/User/UserList', 
        type: 'GET',
        dataType: 'html',
        success: function (data) {
            $('#tblData').html(data); 
        },
        error: function () {
            alert("Error loading user list");
        }
    });
}


function StatusChange(record) {

    $.ajax({
        url: '/User/StatusChange',
        type: 'POST',
        data: record,
        dataType: 'json',

        success: function (response) {
            if (response.success) {
                console.log('Status changed successfully.');
                showUser();


                /*updateSingleRow(record.UserId);*/


            } else {
                console.error('status not saved.');
            }
        },
        error: function (xhr, status, error) {
            console.error('Error occurred while saving Status:', error);
        }
    });
}


$('#btnAdd').click(function () {
    
    Add();
});

var records = [];

function Add() {


    $.ajax({
        url: '/User/AddRowPartial', 
        type: 'GET',
        success: function (partialViewHtml) {

           
         
            $('.container10').append(partialViewHtml);

  
            var lastRow = $('.container form:last');
            LoadCountries(lastRow.find('.country'));

 
            bindEvents();
        },
        error: function (xhr, status, error) {
        
            console.error('Error fetching partial view:', error);
        }
    });
}


function bindEvents() {
  
    $('.country').on('change', function () {
        var selectedCountryId = $(this).val();
        var row = $(this).closest('form');
        var stateDropdown = row.find('.state');

        LoadStates(selectedCountryId, stateDropdown); 

    });


    $('.state').on('change', function () {
        var selectedStateId = $(this).val();
        var row = $(this).closest('form');
        var cityDropdown = row.find('.city');

        LoadCities(selectedStateId, cityDropdown); 
    });


    $('.discard-button').on('click', function () {
        $(this).closest('form').remove();
    });
}

function SaveSingleRecord(formId) {


    var form = $('#' + formId);

    var isValid = form.valid();
    if (isValid) {
        var name = form.find('.name-input').val();
        var countryId = form.find('.country').val();
        var stateId = form.find('.state').val();
        var cityId = form.find('.city').val();

        var record = {
            Name: name,
            CountryId: countryId,
            StateId: stateId,
            CityId: cityId
        };

        $.ajax({
            url: '/User/AddUser',
            type: 'POST',
            data: record,
            success: function (response) {
                if (response.success) {
                    console.log('Record saved successfully.');
                    showUser();

                    form.remove();

                } else {
                    console.error('Failed to save record.');
                }
            },
            error: function (xhr, status, error) {
                console.error('Error occurred while saving record:', error);
            }
        });
    }

}

$('#btnSaveAll').on('click', function () {
    var records = [];
    var isValid = true;
    $('form').each(function (index, item) {
        if (!$(item).valid()) {
            isValid = false;
            records = {};
            return;
        } else {
            var row = $(this);
            var userId = item.userId;
            var record;
            if (userId == null || userId === '') {
                var name = row.find('.name-input').val();
                var countryId = row.find('.country').val();
                var stateId = row.find('.state').val();
                var cityId = row.find('.city').val();
                if (name != undefined) {
                    record = {
                        Name: name,
                        CountryId: countryId,
                        StateId: stateId,
                        CityId: cityId,

                    };
                    records.push(record);
                }
            }
        }
    });

    if (isValid) {
        SaveAll(records);
    }
});




function SaveAll(records) {

    $.ajax({
        url: '/User/SaveAll', 
        type: 'POST',
       data: { jsonData:records} , 
        success: function (response) {

            console.log(response);
            showUser();
            $('.container10').empty();

        },
        error: function (xhr, status, error) {
         
            console.error(xhr.responseText);
        }
    });
}



function LoadCountries(lastcountry) {
    lastcountry.empty();

    $.ajax({
        url: '/User/CountryList',
        success: function (response) {
            if (response != null && response != undefined && response.length > 0) {
                lastcountry.attr('disabled', false);
                lastcountry.append('<option disabled selected>--- Select Country ---</option>');

                $.each(response, function (i, data) {
                    lastcountry.append('<option value="' + data.countryId + '">' + data.countryName + '</option>');
                });

            } else {
                lastcountry.attr('disabled', true);
                lastcountry.append('<option>--- Countries not Available ---</option>');
            }
        }
    });
}

function LoadStates(countryId, stateDropdown) {
    stateDropdown.empty();

    $.ajax({
        url: '/User/StateList?countryId=' + countryId,
        success: function (response) {
            if (response != null && response != undefined && response.length > 0) {
                stateDropdown.prop('disabled', false);
                stateDropdown.empty().append('<option disabled selected>--- Select State ---</option>');

                $.each(response, function (i, data) {
                    var option = $('<option value="' + data.stateId + '">' + data.stateName + '</option>');
                    stateDropdown.append(option);
                });
            }
            else {
                stateDropdown.prop('disabled', true);
                stateDropdown.empty().append('<option>--- States not Available ---</option>');
            }
        }
    });
}

function LoadCities(stateId, cityDropdown) {
    cityDropdown.empty();

    $.ajax({
        url: '/User/CityList?stateId=' + stateId,
        success: function (response) {
            if (response != null && response != undefined && response.length > 0) {
                cityDropdown.prop('disabled', false);
                cityDropdown.empty().append('<option disabled selected>--- Select City ---</option>');

                $.each(response, function (i, data) {
                    var option = $('<option value="' + data.cityId + '">' + data.cityName + '</option>');
                    cityDropdown.append(option);
                });
            }
            else {
                cityDropdown.prop('disabled', true);
                cityDropdown.empty().append('<option>--- Cities not Available ---</option>');
            }
        }
    });
}

$(document).on('change', '.countryDropdown', function () {
    var countryId = $(this).val();
    var stateDropdown = $(this).closest('tr').find('.stateDropdown');
    LoadStates(countryId, stateDropdown);

});

$(document).on('change', '.stateDropdown', function () {
    var stateID = $(this).val();
    var cityDropdown = $(this).closest('tr').find('.cityDropdown');
    LoadCities(stateID, cityDropdown);
});

