$(document).ready(function () {

    $('#executeAnalysis').click(function (event) {
        var selectedValue = $('#imageIds option:selected').text();

        // Execute the AJAX request
        $.ajax({
            url: '/Ai/v1/DetectFoodByMongoImageId/' + selectedValue, // The URL to send the request to
            type: 'POST', // or 'POST', depending on your needs
            success: function (response) {
                $('#visionClassification').val(response);
            },
            error: function (xhr, status, error) {
                // Handle error
                console.log('Error:', error);
            }
        });
    });

    $('#saveClassification').click(function (event) {
        var selectedValue = $('#imageIds option:selected').text();
        var humanClassification = $('#humanClassification').val();

        var itemsCount = $('#itemsCount').val();
        var itemsOkCount = $('#itemsOkCount').val();

        var myData = JSON.stringify({
            HumanClassification: humanClassification,
            ItemsCount: parseInt(itemsCount),
            ItemsOkCount: parseInt(itemsOkCount)
        });

        // Execute the AJAX request
        $.ajax({
            url: '/Image/v1/UpdateHumanClassification/' + selectedValue, // The URL to send the request to
            type: 'PUT', // or 'POST', depending on your needs
            contentType: 'application/json',
            data: myData,
            success: function (response) {
                alert('Classificação humana atualizada com sucesso!');
            },
            error: function (xhr, status, error) {
                // Handle error
                console.log('Error:', error);
            }
        });
    });

    $('#deleteImage').click(function (event) {

        var selectedValue = $('#imageIds option:selected').text();

        // Execute the AJAX request
        $.ajax({
            url: '/Image/v1/DeleteImageById/' + selectedValue, // The URL to send the request to
            type: 'DELETE', // or 'POST', depending on your needs
            success: function (response) {
                $('#imageIds option:selected').remove();
                alert('Imagem deletada com sucesso!');
            },
            error: function (xhr, status, error) {
                // Handle error
                console.log('Error:', error);
            }
        });
    });

    $('#imageIds').change(function () {
        var selectedValue = $(this).val(); // Get the value of the selected option

        // Execute the AJAX request
        $.ajax({
            url: '/Image/v1/GetImageById/' + selectedValue, // The URL to send the request to
            type: 'GET', // or 'POST', depending on your needs
            success: function (response) {
                const imgSrc = `data:image/jpeg;base64,${response.imageBytes}`;
                $('#img').attr('src', imgSrc);

                if (response.humanResult == null)
                    $('#humanClassification').val('');
                else
                    $('#humanClassification').val(response.humanResult);

                if (response.visionRawResult == null)
                    $('#visionClassification').val('');
                else
                    $('#visionClassification').val(response.visionRawResult);

                if (response.totalItems == null)
                    $('#itemsCount').val('');
                else
                    $('#itemsCount').val(response.totalItems);

                if (response.totalOkItems == null)
                    $('#itemsOkCount').val('');
                else
                    $('#itemsOkCount').val(response.totalOkItems);
            },
            error: function (xhr, status, error) {
                // Handle error
                console.log('Error:', error);
            }
        });
    });
});