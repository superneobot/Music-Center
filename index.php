<html>
<head>
<title>My Music Center - плейлист пользователя - kalky</title>
<link rel="stylesheet" href="/css/style.css">
<link rel="stylesheet" href="/css/bootstrap.min.css">
<link rel="stylesheet" href="/css/bootstrap.css">
</head>
<body id="main" style class>
 <nav class="navbar navbar-expand-lg fixed-top navbar-dark bg-dark">
<div class="container-fluid">
<a class="navbar-brand" href="/index.php"><img src="/image/logo.png" width="42" style="margin-right: 10px;"  />Music Center</a>
<button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarColor02" aria-controls="navbarColor02" aria-expanded="false" aria-label="Toggle navigation">
<span class="navbar-toggler-icon"></span>
</button>
<div class="collapse navbar-collapse" id="navbarColor02">
<ul class="navbar-nav me-auto">
<li class="nav-item">
<a class="nav-link" href="/index.php">Главная</a>
</li>
<!-- <li class="nav-item">
<a class="nav-link" href="/registration/reg.php">Регистрация</a>
</li>
 --><li class="nav-item">
<a class="nav-link" href="/files/download.php">Загрузки
</a>
</li>
<li class="nav-item">
<a class="nav-link active" href="/playlist/users.php">Плейлисты<span class="visually-hidden">(current)</span></a>
</li>
</ul>
</div>
</div>
</nav>
<div class="playlist">
<div class="card border-dark mb-3" style="margin:15px; width:900px">
<div class="card-header">Плейлист пользователя - kalky</div>
<div class="card-body" style="justify-content: center;">
<ul class="track__list" id="telo"></ul>
<div class='counts' id='count'></div>
</div>
</div>
<script>
function getFileJson(fileName) {
let request = new XMLHttpRequest();
request.open('GET', fileName, false);
request.send(null);
return JSON.parse(request.responseText);
}
let count = 0;
let data = getFileJson('favorite.data');
data.forEach((item) => {
var info = `{\"artist\":\"${item.Artist}\",\"title\":\"${item.Title}\",\"url\":\"${item.FilePath}\",\"img\":\"${item.Poster}\"}`;
var id = `track-id-${item.Id}`;
let block = document.createElement('li');
block.classList.add('track');
block.setAttribute('data-musmeta', info);
block.setAttribute('data-musid', id);
block.title = item.Title;
block.innerHTML = `
<div class=track__item>
<div class=poster__div style="background-image: url('${item.Poster}');">
<img class=poster__img src=${item.Poster} /></div>
<div class=track__detail>
<div class=track__info>
 <div class=track__title> ${item.Title} </div>
<div class=track__artist>${item.Artist}</div>
</div>
<div class=track__duration>${item.Duration}</div>
</div>
</div>
<br>
`;
count++;
let dl = document.getElementById('telo');
dl.append(block);
});
 </script>
</div>
</body>
<script src="https://bootswatch.com/_vendor/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
<script>
let back = document.querySelector('div.counts');
back.innerHTML = 'Всего ' + count;
</script>
</html>

