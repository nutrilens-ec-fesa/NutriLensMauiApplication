$(function () {
    $.ajax({
        url: '/Image/v1/GetAllImagesIds', // Your API endpoint
        type: 'GET',
        dataType: 'json', // Expecting JSON response
        success: function (data) {
            const imagesGrid = $('#imagesGrid');
            data.forEach((item, index) => {

                // Execute the AJAX request
                $.ajax({
                    url: '/Image/v1/GetImageById/' + item, // The URL to send the request to
                    type: 'GET', // or 'POST', depending on your needs
                    success: function (response) {
                        // Assuming 'imageBytes' is a Base64-encoded image
                        const imgSrc = `data:image/jpeg;base64,${response.imageBytes}`;
                        const imgElement = $('<img id="image-' + index + '" class="dynamic-image">').attr('src', imgSrc).attr('download', item.id).css({
                            width: '100%', // Ensure the image fits its container
                            height: 'auto'
                        });
                        const divElement = $('<div>').attr('class', 'image-container');
                        divElement.append(imgElement);
                        imagesGrid.append(divElement);
                    },
                    error: function (xhr, status, error) {
                        // Handle error
                        console.log('Error:', error);
                    }
                });
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error('Error fetching data: ' + textStatus);
        }
    });
});

$('#imagesGrid').on('click', '.dynamic-image', function () {
    // Assuming the image source can be directly downloaded
    const imageUrl = $(this).attr('src');
    const imageName = 'downloadedImage.jpg'; // Or dynamically generate a name based on the image

    // Create a temporary <a> element
    const link = document.createElement('a');
    link.href = imageUrl;
    link.download = imageName;

    // Append to the body to make it part of the document
    document.body.appendChild(link);

    // Programmatically click the link to trigger the download
    link.click();

    // Remove the temporary link from the document
    document.body.removeChild(link);
    //const imageId = this.id; // Retrieve the ID of the clicked image
    //// Create options dynamically or reveal an existing menu for the clicked image
    //// For demonstration, let's create two simple buttons dynamically:

    //const option1 = $('<button>').text('Option 1').click(function () {
    //    // Define action for Option 1
    //    console.log('Option 1 clicked for', imageId);
    //});

    //const option2 = $('<button>').text('Option 2').click(function () {
    //    // Define action for Option 2
    //    console.log('Option 2 clicked for', imageId);
    //});

    //// For simplicity, we'll append these options directly to the image's parent.
    //// In a real scenario, you might want to manage the positioning and display more carefully.
    //$(this).parent().append(option1, option2);
});