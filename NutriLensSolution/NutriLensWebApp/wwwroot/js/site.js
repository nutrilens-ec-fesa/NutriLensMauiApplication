document.getElementById('selectImageButton').addEventListener('click', function () {
    document.getElementById('fileInput').click();
});

document.getElementById('fileInput').addEventListener('change', function (event) {
    const file = event.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            document.getElementById('img').src = e.target.result;
        };
        reader.readAsDataURL(file);
    }
});

try {
    document.getElementById('sendGptRequestButton').addEventListener('click', function (event) {

        let base64ImgString;

        convertImageToBase64('img', function (base64Img) {
            base64ImgString = base64Img;
        });

        var systemPromptVal = $('#systemPrompt').val();
        var userPromptVal = $('#userPrompt').val();
        var maxTokensVal = parseInt($('#maxTokens').val(), 10);

        var openAiVisionInputModel = {
            systemPrompt: systemPromptVal,
            userPrompt: userPromptVal,
            maxTokens: maxTokensVal,
            url: base64ImgString,
            base64: false
        };

        $('#resultParagraph').text('Enviando requisição...');
        // Send the object as a JSON string in a POST request
        $.ajax({
            url: '/Ai/v1/Gpt4VisionTest', // Replace with your target URL
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(openAiVisionInputModel),
            success: function (response) {
                // Handle success
                console.log('Data sent successfully');
                console.log(response);

                $('#resultParagraph').text(response);
            },
            error: function (xhr, status, error) {
                // Handle error
                console.error('Error sending data');
                $('#resultParagraph').text('Houve alguma falha para enviar a solicitacao, tente novamente...');
            }
        });
    });
}
catch {

}

try {
    document.getElementById('sendGeminiRequestButton').addEventListener('click', function (event) {

        let base64ImgString;

        convertImageToBase64('img', function (base64Img) {
            base64ImgString = base64Img;
        });

        var userPromptVal = $('#userPrompt').val();

        var geminiVisionInputModel = {
            prompt: userPromptVal,
            url: base64ImgString
        };

        $('#resultParagraph').text('Enviando requisição...');
        // Send the object as a JSON string in a POST request
        $.ajax({
            url: '/Ai/v1/GeminiVisionTest', // Replace with your target URL
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(geminiVisionInputModel),
            success: function (response) {
                // Handle success
                console.log('Data sent successfully');
                console.log(response);

                $('#resultParagraph').text(response);
            },
            error: function (xhr, status, error) {
                // Handle error
                console.error('Error sending data');
                $('#resultParagraph').text('Houve alguma falha para enviar a solicitacao, tente novamente...');
            }
        });
    });
}
catch {

}

document.getElementById('getLastNutriLensPrompt').addEventListener('click', function (event) {
    $.ajax({
        url: '/Ai/v1/GetActualGpt4VisionPrompt', // Replace with your target URL
        type: 'GET',
        contentType: 'application/json',
        success: function (response) {
            // Handle success
            console.log('Data sent successfully');
            console.log(response);

            $('#systemPrompt').text(response.systemPrompt);
            $('#userPrompt').text(response.userPrompt);

            alert('Prompt resgatado com sucesso!');
        },
        error: function (xhr, status, error) {
            // Handle error
            console.error('Error sending data');
            $('#resultParagraph').text('Houve alguma falha para enviar a solicitacao, tente novamente...');
        }
    });
});

document.getElementById('saveNutriLensPrompt').addEventListener('click', function (event) {

    var systemPromptVal = $('#systemPrompt').val();
    var userPromptVal = $('#userPrompt').val();

    var openAiVisionInputModel = {
        systemPrompt: systemPromptVal,
        userPrompt: userPromptVal
    };

    $('#resultParagraph').text('Enviando requisição...');
    // Send the object as a JSON string in a POST request
    $.ajax({
        url: '/Ai/v1/InsertNewGpt4VisionPrompt', // Replace with your target URL
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(openAiVisionInputModel),
        success: function (response) {
            // Handle success
            console.log('Data sent successfully');
            console.log(response);

            alert('Prompt salvo com sucesso!');
        },
        error: function (xhr, status, error) {
            // Handle error
            console.error('Error sending data');
            $('#resultParagraph').text('Houve alguma falha para enviar a solicitacao, tente novamente...');
        }
    });
});

function convertImageToBase64(imgElementId, callback) {
    // Get the image element by its ID
    const img = document.getElementById(imgElementId);

    // Create an off-screen canvas
    const canvas = document.createElement('canvas');
    canvas.width = img.width;
    canvas.height = img.height;

    // Draw the image onto the canvas
    const ctx = canvas.getContext('2d');
    ctx.drawImage(img, 0, 0);

    // Convert the canvas to a data URL and call the callback with the result
    const dataURL = canvas.toDataURL('image/png');

    // Call the provided callback function with the base64 string
    callback(dataURL);
}