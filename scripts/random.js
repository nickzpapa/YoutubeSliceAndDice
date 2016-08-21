
$('#start').click(function () {
    if ($('#url').val() != "") {
        if ($('#tracklist').val() != '' || $('#tracklist').val() != '') {
            var url = $('#url').val();
            var artist = $('#artist').val();
            var artist = $('#album').val();
            var tracks = $('#tracklist').val();
            var data = { 'url': url, 'tracklist': tracks, 'artist': artist, 'album': album };
            console.log(data);
        }
        $("#status").html($("#status").html() + '<br>Starting album download...');
        $('#start').html("Downloading...");
    }
});

function randomColor() {
    var newColor = '#' + (0x1000000 + (Math.random()) * 0xffffff).toString(16).substr(1, 6);
    $('.form-control:focus').css('border-color', newColor);
    $('#status').css('border-color', newColor);
    $('.btn-primary').css('background-image', 'linear-gradient(to bottom,' + newColor + ', ' + newColor + ')');
    $('.btn-primary').css('border-color', newColor);
    $('#status').css('border-color', newColor);
    $('#status').css('color', newColor);
    $('body').css('background-color', newColor);
    $('a').css('color', newColor);
    $('::selection').css('background', newColor);
    $('::-moz-selection').css('background', newColor);
    $('.form-control:focus').css('border-color', newColor);
    $('.form-control').css('border', '1px solid ' + newColor);
    $('.form-control').css('color', newColor);
    $('.form-control::-moz-placeholder').css('color', newColor);
}