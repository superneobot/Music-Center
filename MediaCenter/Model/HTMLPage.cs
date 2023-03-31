using System.IO;

namespace MediaCenter.Model {
    public static class HTMLPage {
        public static void Create(string user, string path) {
            string[] telo = new string[] {
            "<html>\r\n<head>" +
            "<meta charset='utf-8'>\r\n" +
            "<link rel=\"icon\" type=\"image/ico\" href=\"favicon.ico\" />\r\n" +
            "<meta name='viewport' content='width=device-width, initial-scale=1'>\r\n" +
            "<link rel=\"stylesheet\" type=\"text/css\" href=\"/style/style.css\">\r\n" +
            "<link href='https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta1/dist/css/bootstrap.min.css' rel='stylesheet'>\r\n" +
            "<link href='https://cdn.jsdelivr.net/npm/boxicons@latest/css/boxicons.min.css' rel='stylesheet'>\r\n  " +
            "<script type='text/javascript' src='https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js'></script>\r\n" +
           $"<title>My Music Center - плейлист пользователя [{user}]</title>\r\n   " +
            "<style>\r\n        " +
            "::-webkit-scrollbar " +
            "{\r\n            " +
            "width: 8px;\r\n        " +
            "}\r\n\r\n        " +
            "/* Track */\r\n        " +
            "::-webkit-scrollbar-track {\r\n            " +
            "background: #f1f1f1;\r\n        " +
            "}\r\n\r\n        " +
            "/* Handle */\r\n        " +
            "::-webkit-scrollbar-thumb " +
            "{\r\n            " +
            "background: #888;\r\n        " +
            "}\r\n\r\n        " +
            "/* Handle on hover */\r\n        " +
            "::-webkit-scrollbar-thumb:hover {\r\n            " +
            "background: #555;\r\n        " +
            "}\r\n\r\n        " +
            "@import url(\"https://fonts.googleapis.com/css2?family=Nunito:wght@400;600;700&display=swap\");\r\n\r\n        " +
            ":root {\r\n            " +
            "--header-height: 3rem;\r\n            " +
            "--nav-width: 68px;\r\n            " +
            "--first-color: #222;\r\n            " +
            "--first-color-light: #ccc;\r\n            " +
            "--white-color: #ffffff;\r\n           " +
            " --body-font: 'Motiva Sans', sans-serif;\r\n            " +
            "--normal-font-size: 1rem;\r\n            " +
            "--z-fixed: 100;\r\n            " +
            "--background-image: url('/image/background_fon.svg');\r\n       " +
            " }\r\n\r\n        " +
            "*,\r\n        " +
            "::before,\r\n        " +
            "::after {\r\n            " +
            "box-sizing: border-box\r\n       " +
            " }\r\n\r\n        " +
            "body {\r\n            " +
            "position: absolute;\r\n            " +
            "margin: var(--header-height) 0 0 0;\r\n            " +
            "padding: 0 1rem;\r\n            " +
            "font-family: var(--body-font);\r\n            " +
            "font-size: var(--normal-font-size);\r\n            " +
            "transition: .5s;\r\n           " +
            "background-image: var(--background-image);\r\n            " +
            "background-attachment: fixed;\r\n        " +
            "}\r\n\r\n       " +
            " a {\r\n            " +
            "text-decoration: none\r\n        " +
            "}\r\n\r\n        " +
            ".header {\r\n            " +
            "width: 100%;\r\n            " +
            "height: var(--header-height);\r\n           " +
            "position: fixed;\r\n            " +
            "top: 0;\r\n            " +
            "left: 0;\r\n            " +
            "display: flex;\r\n            " +
            "align-items: center;\r\n            " +
            "justify-content: space-between;\r\n            " +
            "padding: 0 1rem;\r\n            " +
            "background-color: var(--white-color);\r\n            " +
            "z-index: var(--z-fixed);\r\n            " +
            "transition: .5s\r\n        " +
            "}\r\n\r\n        " +
            ".header_toggle {\r\n            " +
            "color: var(--first-color);\r\n            " +
            "font-size: 1.5rem;\r\n            " +
            "cursor: pointer\r\n        " +
            "}\r\n\r\n        " +
            ".header_img {\r\n            " +
            "width: 35px;\r\n           " +
            "height: 35px;\r\n            " +
            "display: flex;\r\n            " +
            "justify-content: center;\r\n            " +
            "border-radius: 50%;\r\n            " +
            "overflow: hidden\r\n        " +
            "}\r\n\r\n        " +
            ".header_img img {\r\n            " +
            "width: 40px\r\n        " +
            "}\r\n\r\n        " +
            ".l-navbar {\r\n            " +
            "position: fixed;\r\n            " +
            "top: 0;\r\n            " +
            "left: -30%;\r\n            " +
            "width: var(--nav-width);\r\n            " +
            "height: 100vh;\r\n            " +
            "background-color: var(--first-color);\r\n            " +
            "padding: .5rem 1rem 0 0;\r\n            " +
            "transition: .5s;\r\n            " +
            "z-index: var(--z-fixed)\r\n       " +
            "}\r\n\r\n        " +
            ".nav {\r\n            " +
            "height: 100%;\r\n            " +
            "display: flex;\r\n            " +
            "flex-direction: column;\r\n            " +
            "justify-content: space-between;\r\n            " +
            "overflow: hidden\r\n        " +
            "}\r\n\r\n        " +
            ".nav_logo,\r\n        " +
            ".nav_link {\r\n            " +
            "display: grid;\r\n            " +
            "grid-template-columns: max-content max-content;\r\n            " +
            "align-items: center;\r\n            " +
            "column-gap: 1rem;\r\n            " +
            "padding: .5rem 0 .5rem 1.5rem\r\n        " +
            "}\r\n\r\n        " +
            ".nav_logo {\r\n            " +
            "margin-bottom: 2rem\r\n        " +
            "}\r\n\r\n        " +
            ".nav_logo-icon {\r\n           " +
            " font-size: 1.25rem;\r\n            " +
            "color: var(--white-color)\r\n       " +
            " }\r\n\r\n        " +
            ".nav_logo-name {\r\n            " +
            "color: var(--white-color);\r\n            " +
            "font-weight: 700\r\n        " +
            "}\r\n\r\n        " +
            ".nav_link {\r\n            " +
            "position: relative;\r\n            " +
            "color: var(--first-color-light);\r\n            " +
            "margin-bottom: 1.5rem;\r\n            " +
            "transition: .3s\r\n        }\r\n\r\n        " +
            ".nav_link:hover {\r\n            " +
            "color: var(--white-color)\r\n       " +
            "}\r\n\r\n        " +
            ".nav_icon {\r\n            " +
            "font-size: 1.25rem\r\n        " +
            "}\r\n\r\n        " +
            ".show {\r\n            " +
            "left: 0\r\n        " +
            "}\r\n\r\n        " +
            ".body-pd {\r\n            " +
            "padding-left: calc(var(--nav-width) + 1rem);\r\n        " +
            "}\r\n\r\n        " +
            ".active {\r\n            " +
            "color: var(--white-color)\r\n        " +
            "}\r\n\r\n        " +
            ".active::before {\r\n            " +
            "content: '';\r\n            " +
            "position: absolute;\r\n            " +
            "left: 0;\r\n           " +
            "width: 2px;\r\n            " +
            "height: 32px;\r\n            " +
            "background-color: var(--white-color)\r\n        " +
            "}\r\n\r\n        " +
            ".height-100 {\r\n            " +
            "height: 100vh\r\n        " +
            "}\r\n\r\n        " +
            "@media screen and (min-width: 768px) {\r\n           " +
            "body {\r\n                " +
            "margin: calc(var(--header-height) + 1rem) 0 0 0;\r\n                " +
            "padding-left: calc(var(--nav-width) + 2rem)\r\n            " +
            "}\r\n\r\n            " +
            ".header {\r\n                " +
            "height: calc(var(--header-height) + 1rem);\r\n                " +
            "padding: 0 2rem 0 calc(var(--nav-width) + 2rem)\r\n            " +
            "}\r\n\r\n            " +
            ".header_img {\r\n                " +
            "width: 40px;\r\n                " +
            "height: 40px\r\n            " +
            "}\r\n\r\n            " +
            ".header_img img {\r\n               " +
            "width: 45px\r\n            " +
            "}\r\n\r\n            " +
            ".l-navbar {\r\n                " +
            "left: 0;\r\n                " +
            "padding: 1rem 1rem 0 0\r\n            " +
            "}\r\n\r\n            " +
            ".show {\r\n                " +
            "width: calc(var(--nav-width) + 156px)\r\n           " +
            "}\r\n\r\n            " +
            ".body-pd {\r\n               " +
            "padding-left: calc(var(--nav-width) + 188px)\r\n           " +
            " }\r\n        " +
            "}\r\n    " +
            "</style>\r\n" +
            "</head>\r\n" +
            "<body id=\"body-pd\">\r\n    " +
            "<header class=\"header\" id=\"header\">\r\n        " +
            "<div class=\"header_toggle\">\r\n            " +
            "<i class='bx bx-menu' id=\"header-toggle\"></i>\r\n        " +
            "</div>\r\n       " +
            "<div class=\"header_img\">\r\n           " +
            "<img src=\"/image/logo.png\" alt=\"\">\r\n        " +
            "</div>\r\n   " +
            "</header>\r\n    " +
            "<div class=\"l-navbar\" id=\"nav-bar\">\r\n       " +
            "<nav class=\"nav\">\r\n           " +
            "<div>\r\n               " +
            "<a href=\"/index.html\" class=\"nav_logo\"> <i class='bx bx-layer nav_logo-icon'\r\n title=\"Главная страница\"></i>\r\n                    " +
            "<span class=\"nav_logo-name\">Music Center</span>\r\n               " +
            "</a>\r\n                " +
            "<div class=\"nav_list\">\r\n                    " +
            "<a href=\"/index.html\" class=\"nav_link\">\r\n                       " +
            "<i class='bx bx-grid-alt nav_icon' title=\"Главная страница\"></i>\r\n                       " +
            "<span class=\"nav_name\">Главная страница</span>\r\n                    " +
            "</a>\r\n                    " +
            "<a href=\"/playlist/index.html\" class=\"nav_link active\">\r\n                        " +
            "<i class='bx bx-message-square-detail nav_icon' title=\"Плейлисты пользователей\"></i>\r\n                       " +
            "<span class=\"nav_name\">Плейлисты</span>\r\n                   " +
            "</a>\r\n                   " +
            "<a href=\"/files/index.html\" class=\"nav_link\">\r\n                        " +
            "<i class='bx bx-folder nav_icon' title=\"Загрузки\"></i>\r\n                       " +
            "<span class=\"nav_name\">Загрузки</span>\r\n                    " +
            "</a>\r\n                " +
            "</div>\r\n           " +
            "</div>\r\n           " +
            "<a href=\"#\" class=\"nav_link\">\r\n               " +
            "<i class='bx bx-log-out nav_icon'></i>\r\n                " +
            "<span class=\"nav_name\">Вход</span>\r\n           " +
            "</a>\r\n        " +
            "</nav>\r\n   " +
            "</div>\r\n   " +
            $"<div class=\"caption\">Плейлист пользователя - {user}</div>\r\n       " +
            "<div class=\"content-item tracks\" id=\"telo\" source=\"favorite.data\">\r\n          " +
            "<div class=\"dl-caption\">Список треков</div>\r\n           " +
            "<ul class=tracks__list>\r\n                " +
            "<script>\r\n                    " +
            "function getFileJson(fileName) {\r\n                        " +
            "let request = new XMLHttpRequest();\r\n                       " +
            "request.open('GET', fileName, false);\r\n                       " +
            "request.send(null);\r\n                       " +
            "return JSON.parse(request.responseText);\r\n                 " +
            "}\r\n                    let count = 0;\r\n                 " +
            "let source = document.querySelector('div#telo.content-item.tracks');\r\n      " +
            "var path = source.getAttribute('source');\r\n                    " +
            "let data = getFileJson(path);\r\n                    " +
            "data.forEach((item) => {\r\n                        " +
            "var info = `{\\\"artist\\\":\\\"${item.Artist}\\\",\\\"title\\\":\\\"${item.Title}\\\",\\\"url\\\":\\\"${item.FilePath}\\\",\\\"img\\\":\\\"${item.Poster}\\\"}`;\r\n                        " +
            "var id = `track-id-${item.Id}`;\r\n                       " +
            "let block = document.createElement('li');\r\n                       " +
            "block.classList.add('track');\r\n                        " +
            "block.setAttribute('data-musmeta', info);\r\n                        " +
            "block.setAttribute('data-musid', id);\r\n                        " +
            "block.title = item.Title;\r\n                        " +
            "block.innerHTML = ` \r\n                    " +
            "<div class=track__item>  \r\n                        " +
            "<div class=poster__div style=\"background-image: url('${item.Poster}');\">\r\n                            " +
            "<img class=poster__img src=${item.Poster} /></div>\r\n                       " +
            "<div class=track__detail>\r\n                            " +
            "<div class=track__info>\r\n                            " +
            "<div class=track__title> ${item.Title} </div>\r\n                           " +
            "<div class=track__artist>${item.Artist}</div>\r\n                            " +
            "</div>\r\n                            " +
            "<div class=track__duration>${item.Duration}</div>\r\n                        " +
            "</div>\r\n                    " +
            "</div>\r\n        `;\r\n                        " +
            "count++;\r\n                        " +
            "let dl = document.getElementById('telo');\r\n                        " +
            "dl.append(block);\r\n                   " +
            "});\r\n               " +
            "</script>\r\n            " +
            "</ul>\r\n           " +
            "<div class=\"tracks_down\">\r\n                " +
            "<script>\r\n                    " +
            "let back = document.querySelector('div.tracks_down');\r\n                    " +
            "back.innerHTML = 'Всего ' + count;\r\n                " +
            "</script>\r\n            " +
            "</div>\r\n        " +
            "</div>\r\n    " +
            "</div>\r\n    " +
            "<script src=\"https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js\"></script>\r\n    " +
            "<script src=\"https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta1/dist/js/bootstrap.bundle.min.js\"></script>\r\n    " +
            "<script src=\"/js/script.js\"></script>\r\n    " +
            "<script type='text/javascript'>" +
            "document.addEventListener(\"DOMContentLoaded\", function (event) {\r\n\r\n            " +
            "const showNavbar = (toggleId, navId, bodyId, headerId) => {\r\n                " +
            "const toggle = document.getElementById(toggleId),\r\n                    " +
            "nav = document.getElementById(navId),\r\n                    " +
            "bodypd = document.getElementById(bodyId),\r\n                    " +
            "headerpd = document.getElementById(headerId)\r\n\r\n                " +
            "// Validate that all variables exist\r\n                " +
            "if (toggle && nav && bodypd && headerpd) {\r\n                    " +
            "toggle.addEventListener('click', () => {\r\n                        " +
            "// show navbar\r\n                        " +
            "nav.classList.toggle('show')\r\n                        " +
            "// change icon\r\n                        " +
            "toggle.classList.toggle('bx-x')\r\n                        " +
            "// add padding to body\r\n                        " +
            "bodypd.classList.toggle('body-pd')\r\n                       " +
            "// add padding to header\r\n                        " +
            "headerpd.classList.toggle('body-pd')\r\n                    " +
            "})\r\n                " +
            "}\r\n            " +
            "}\r\n\r\n            " +
            "showNavbar('header-toggle', 'nav-bar', 'body-pd', 'header')\r\n\r\n            " +
            "/*===== LINK ACTIVE =====*/\r\n            " +
            "const linkColor = document.querySelectorAll('.nav_link')\r\n\r\n           " +
            "function colorLink() {\r\n                " +
            "if (linkColor) {\r\n                    " +
            "linkColor.forEach(l => l.classList.remove('active'))\r\n                   " +
            "this.classList.add('active')\r\n               " +
            "}\r\n           " +
            "}\r\n            " +
            "linkColor.forEach(l => l.addEventListener('click', colorLink))\r\n\r\n            " +
            "// Your code to run since DOM is loaded and ready\r\n        " +
            "});</script>\r\n    " +
            "<script type='text/javascript'>var myLink = document.querySelector('a[href=\"#\"]');\r\n        " +
            "myLink.addEventListener('click', function (e) {\r\n            " +
            "e.preventDefault();\r\n        " +
            "});</script>"+
            "</body>\r\n\r\n" +
            "</html>"
        };
            string[] page = new string[] {
                "<html>\r\n" +
                "<head>\r\n" +
                $"<title>My Music Center - плейлист пользователя - {user}</title>\r\n" +
                "<link rel=\"stylesheet\" href=\"/css/style.css\">\r\n" +
                "<link rel=\"stylesheet\" href=\"/css/bootstrap.min.css\">\r\n" +
                "<link rel=\"stylesheet\" href=\"/css/bootstrap.css\">\r\n" +
                "</head>\r\n<body id=\"main\" style class>\r\n " +
                "<nav class=\"navbar navbar-expand-lg fixed-top navbar-dark bg-dark\">\r\n" +
                "<div class=\"container-fluid\">\r\n" +
                "<a class=\"navbar-brand\" href=\"/index.php\"><img src=\"/image/logo.png\" width=\"42\" style=\"margin-right: 10px;\"  />Music Center</a>\r\n" +
                "<button class=\"navbar-toggler\" type=\"button\" data-bs-toggle=\"collapse\" data-bs-target=\"#navbarColor02\" aria-controls=\"navbarColor02\" aria-expanded=\"false\" aria-label=\"Toggle navigation\">\r\n" +
                "<span class=\"navbar-toggler-icon\"></span>\r\n" +
                "</button>\r\n" +
                "<div class=\"collapse navbar-collapse\" id=\"navbarColor02\">\r\n" +
                "<ul class=\"navbar-nav me-auto\">\r\n" +
                "<li class=\"nav-item\">\r\n" +
                "<a class=\"nav-link\" href=\"/index.php\">Главная</a>\r\n" +
                "</li>\r\n" +
                "<!-- <li class=\"nav-item\">\r\n" +
                "<a class=\"nav-link\" href=\"/registration/reg.php\">Регистрация</a>\r\n" +
                "</li>\r\n -->" +
                "<li class=\"nav-item\">\r\n" +
                "<a class=\"nav-link\" href=\"/files/download.php\">Загрузки\r\n" +
                "</a>\r\n" +
                "</li>\r\n" +
                "<li class=\"nav-item\">\r\n" +
                "<a class=\"nav-link active\" href=\"/playlist/users.php\">Плейлисты<span class=\"visually-hidden\">(current)</span></a>\r\n" +
                "</li>\r\n" +
                "</ul>\r\n" +
                "</div>\r\n" +
                "</div>\r\n" +
                "</nav>\r\n" +
                "<div class=\"playlist\">\r\n" +
                "<div class=\"card border-dark mb-3\" style=\"margin:15px; width:900px\">\r\n" +
                $"<div class=\"card-header\">Плейлист пользователя - {user}</div>\r\n" +
                "<div class=\"card-body\" style=\"justify-content: center;\">\r\n" +
                "<ul class=\"track__list\" id=\"telo\"></ul>\r\n" +
                "<div class='counts' id='count'></div>\r\n</div>\r\n" +
                "</div>\r\n" +
                "<script>\r\n" +
                "function getFileJson(fileName) {\r\n" +
                "let request = new XMLHttpRequest();\r\n" +
                "request.open('GET', fileName, false);\r\n" +
                "request.send(null);\r\n" +
                "return JSON.parse(request.responseText);\r\n" +
                "}\r\n" +
                "let count = 0;\r\n" +
                "let data = getFileJson('favorite.data');\r\n" +
                "data.forEach((item) => {\r\n" +
                "var info = `{\\\"artist\\\":\\\"${item.Artist}\\\",\\\"title\\\":\\\"${item.Title}\\\",\\\"url\\\":\\\"${item.FilePath}\\\",\\\"img\\\":\\\"${item.Poster}\\\"}`;\r\n" +
                "var id = `track-id-${item.Id}`;\r\n" +
                "let block = document.createElement('li');\r\n" +
                "block.classList.add('track');\r\n" +
                "block.setAttribute('data-musmeta', info);\r\n" +
                "block.setAttribute('data-musid', id);\r\n" +
                "block.title = item.Title;\r\n" +
                "block.innerHTML = `\r\n" +
                "<div class=track__item>\r\n" +
                "<div class=poster__div style=\"background-image: url('${item.Poster}');\">\r\n" +
                "<img class=poster__img src=${item.Poster} /></div>\r\n" +
                "<div class=track__detail>\r\n" +
                "<div class=track__info>\r\n " +
                "<div class=track__title> ${item.Title} </div>\r\n" +
                "<div class=track__artist>${item.Artist}</div>\r\n" +
                "</div>\r\n" +
                "<div class=track__duration>${item.Duration}</div>\r\n" +
                "</div>\r\n" +
                "</div>\r\n" +
                "<br>\r\n" +
                "`;\r\n" +
                "count++;\r\n" +
                "let dl = document.getElementById('telo');\r\n" +
                "dl.append(block);\r\n" +
                "});\r\n " +
                "</script>\r\n" +
                "</div>\r\n" +
                "</body>\r\n" +
                "<script src=\"https://bootswatch.com/_vendor/bootstrap/dist/js/bootstrap.bundle.min.js\"></script>\r\n" +
                "<script>\r\n" +
                "let back = document.querySelector('div.counts');\r\n" +
                "back.innerHTML = 'Всего ' + count;\r\n" +
                "</script>\r\n" +
                "</html>\r\n"
            };
            File.WriteAllLines(path, page);
        }

        static string[] telo = new string[] {
          "<html>\r\n<head>\r\n   " +
            " <meta charset='utf-8'>\r\n   " +
            " <link rel=\"icon\" type=\"image/ico\" href=\"favicon.ico\" />\r\n  " +
            " <meta name='viewport' content='width=device-width, initial-scale=1'>\r\n    " +
            " <link rel=\"stylesheet\" type=\"text/css\" href=\"/style/style.css\">\r\n   " +
            " <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta1/dist/css/bootstrap.min.css' rel='stylesheet'>\r\n  " +
            " <link href='https://cdn.jsdelivr.net/npm/boxicons@latest/css/boxicons.min.css' rel='stylesheet'>\r\n  " +
            " <script type='text/javascript' src='https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js'></script>\r\n    " +
            " <title>My Music Center - плейлист пользователя [kalky]</title>\r\n   " +
            " <style>\r\n        ::-webkit-scrollbar {\r\n            width: 8px;\r\n        }\r\n\r\n        /* Track */\r\n        ::-webkit-scrollbar-track {\r\n            background: #f1f1f1;\r\n        }\r\n\r\n        /* Handle */\r\n        ::-webkit-scrollbar-thumb {\r\n            background: #888;\r\n        }\r\n\r\n        /* Handle on hover */\r\n        ::-webkit-scrollbar-thumb:hover {\r\n            background: #555;\r\n        }\r\n\r\n        @import url(\"https://fonts.googleapis.com/css2?family=Nunito:wght@400;600;700&display=swap\");\r\n\r\n        :root {\r\n            --header-height: 3rem;\r\n            --nav-width: 68px;\r\n            --first-color: #222;\r\n            --first-color-light: #ccc;\r\n            --white-color: #ffffff;\r\n            --body-font: 'Motiva Sans', sans-serif;\r\n            --normal-font-size: 1rem;\r\n            --z-fixed: 100;\r\n            --background-image: url('/image/background_fon.svg');\r\n        }\r\n\r\n        *,\r\n        ::before,\r\n        ::after {\r\n            box-sizing: border-box\r\n        }\r\n\r\n        body {\r\n            position: absolute;\r\n            margin: var(--header-height) 0 0 0;\r\n            padding: 0 1rem;\r\n            font-family: var(--body-font);\r\n            font-size: var(--normal-font-size);\r\n            transition: .5s;\r\n            background-image: var(--background-image);\r\n            background-attachment: fixed;\r\n        }\r\n\r\n        a {\r\n            text-decoration: none\r\n        }\r\n\r\n        .header {\r\n            width: 100%;\r\n            height: var(--header-height);\r\n            position: fixed;\r\n            top: 0;\r\n            left: 0;\r\n            display: flex;\r\n            align-items: center;\r\n            justify-content: space-between;\r\n            padding: 0 1rem;\r\n            background-color: var(--white-color);\r\n            z-index: var(--z-fixed);\r\n            transition: .5s\r\n        }\r\n\r\n        .header_toggle {\r\n            color: var(--first-color);\r\n            font-size: 1.5rem;\r\n            cursor: pointer\r\n        }\r\n\r\n        .header_img {\r\n            width: 35px;\r\n            height: 35px;\r\n            display: flex;\r\n            justify-content: center;\r\n            border-radius: 50%;\r\n            overflow: hidden\r\n        }\r\n\r\n        .header_img img {\r\n            width: 40px\r\n        }\r\n\r\n        .l-navbar {\r\n            position: fixed;\r\n            top: 0;\r\n            left: -30%;\r\n            width: var(--nav-width);\r\n            height: 100vh;\r\n            background-color: var(--first-color);\r\n            padding: .5rem 1rem 0 0;\r\n            transition: .5s;\r\n            z-index: var(--z-fixed)\r\n        }\r\n\r\n        .nav {\r\n            height: 100%;\r\n            display: flex;\r\n            flex-direction: column;\r\n            justify-content: space-between;\r\n            overflow: hidden\r\n        }\r\n\r\n        .nav_logo,\r\n        .nav_link {\r\n            display: grid;\r\n            grid-template-columns: max-content max-content;\r\n            align-items: center;\r\n            column-gap: 1rem;\r\n            padding: .5rem 0 .5rem 1.5rem\r\n        }\r\n\r\n        .nav_logo {\r\n            margin-bottom: 2rem\r\n        }\r\n\r\n        .nav_logo-icon {\r\n            font-size: 1.25rem;\r\n            color: var(--white-color)\r\n        }\r\n\r\n        .nav_logo-name {\r\n            color: var(--white-color);\r\n            font-weight: 700\r\n        }\r\n\r\n        .nav_link {\r\n            position: relative;\r\n            color: var(--first-color-light);\r\n            margin-bottom: 1.5rem;\r\n            transition: .3s\r\n        }\r\n\r\n        .nav_link:hover {\r\n            color: var(--white-color)\r\n        }\r\n\r\n        .nav_icon {\r\n            font-size: 1.25rem\r\n        }\r\n\r\n        .show {\r\n            left: 0\r\n        }\r\n\r\n        .body-pd {\r\n            padding-left: calc(var(--nav-width) + 1rem);\r\n        }\r\n\r\n        .active {\r\n            color: var(--white-color)\r\n        }\r\n\r\n        .active::before {\r\n            content: '';\r\n            position: absolute;\r\n            left: 0;\r\n            width: 2px;\r\n            height: 32px;\r\n            background-color: var(--white-color)\r\n        }\r\n\r\n        .height-100 {\r\n            height: 100vh\r\n        }\r\n\r\n        @media screen and (min-width: 768px) {\r\n            body {\r\n                margin: calc(var(--header-height) + 1rem) 0 0 0;\r\n                padding-left: calc(var(--nav-width) + 2rem)\r\n            }\r\n\r\n            .header {\r\n                height: calc(var(--header-height) + 1rem);\r\n                padding: 0 2rem 0 calc(var(--nav-width) + 2rem)\r\n            }\r\n\r\n            .header_img {\r\n                width: 40px;\r\n                height: 40px\r\n            }\r\n\r\n            .header_img img {\r\n                width: 45px\r\n            }\r\n\r\n            .l-navbar {\r\n                left: 0;\r\n                padding: 1rem 1rem 0 0\r\n            }\r\n\r\n            .show {\r\n                width: calc(var(--nav-width) + 156px)\r\n            }\r\n\r\n            .body-pd {\r\n                padding-left: calc(var(--nav-width) + 188px)\r\n            }\r\n        }\r\n    </style>\r\n" +
            "</head>\r\n" +
            "<body id=\"body-pd\">\r\n    " +
            "<header class=\"header\" id=\"header\">\r\n        " +
            "<div class=\"header_toggle\">\r\n            " +
            "<i class='bx bx-menu' id=\"header-toggle\"></i>\r\n        " +
            "</div>\r\n       " +
            " <div class=\"header_img\">\r\n           " +
            " <img src=\"/image/logo.png\" alt=\"\">\r\n        " +
            "</div>\r\n   " +
            " </header>\r\n    " +
            "<div class=\"l-navbar\" id=\"nav-bar\">\r\n       " +
            " <nav class=\"nav\">\r\n           " +
            " <div>\r\n               " +
            " <a href=\"/index.html\" class=\"nav_logo\"> <i class='bx bx-layer nav_logo-icon'\r\n title=\"Главная страница\"></i>\r\n                    " +
            "<span class=\"nav_logo-name\">Music Center</span>\r\n               " +
            " </a>\r\n                " +
            "<div class=\"nav_list\">\r\n                    " +
            "<a href=\"/index.html\" class=\"nav_link\">\r\n                       " +
            " <i class='bx bx-grid-alt nav_icon' title=\"Главная страница\"></i>\r\n                       " +
            " <span class=\"nav_name\">Главная страница</span>\r\n                    " +
            "</a>\r\n                    " +
            "<a href=\"/playlist/index.html\" class=\"nav_link active\">\r\n                        " +
            "<i class='bx bx-message-square-detail nav_icon' title=\"Плейлисты пользователей\"></i>\r\n                       " +
            " <span class=\"nav_name\">Плейлисты</span>\r\n                   " +
            " </a>\r\n                   " +
            " <a href=\"/files/index.html\" class=\"nav_link\">\r\n                        " +
            "<i class='bx bx-folder nav_icon' title=\"Загрузки\"></i>\r\n                       " +
            " <span class=\"nav_name\">Загрузки</span>\r\n                    " +
            "</a>\r\n                " +
            "</div>\r\n           " +
            " </div>\r\n           " +
            " <a href=\"#\" class=\"nav_link\">\r\n               " +
            " <i class='bx bx-log-out nav_icon'></i>\r\n                " +
            "<span class=\"nav_name\">Вход</span>\r\n           " +
            " </a>\r\n        " +
            "</nav>\r\n   " +
            " </div>\r\n   " +
            " <div class=\"caption\">Плейлисты пользователей</div>\r\n       " +
            " <div class=\"content-item tracks\" id=\"telo\" source=\"favorite.data\">\r\n          " +
            " <div class=\"dl-caption\">Список треков</div>\r\n           " +
            " <ul class=tracks__list>\r\n                " +
            "<script>\r\n                    " +
            "function getFileJson(fileName) {\r\n                        " +
            "let request = new XMLHttpRequest();\r\n                       " +
            " request.open('GET', fileName, false);\r\n                       " +
            " request.send(null);\r\n                       " +
            " return JSON.parse(request.responseText);\r\n                 " +
            "   }\r\n                    let count = 0;\r\n                 " +
            "   let source = document.querySelector('div#telo.content-item.tracks');\r\n      " +
            "   var path = source.getAttribute('source');\r\n                    " +
            "let data = getFileJson(path);\r\n                    " +
            "data.forEach((item) => {\r\n                        " +
            "var info = `{\\\"artist\\\":\\\"${item.Artist}\\\",\\\"title\\\":\\\"${item.Title}\\\",\\\"url\\\":\\\"${item.FilePath}\\\",\\\"img\\\":\\\"${item.Poster}\\\"}`;\r\n                        " +
            "var id = `track-id-${item.Id}`;\r\n                       " +
            "let block = document.createElement('li');\r\n                       " +
            "block.classList.add('track');\r\n                        " +
            "block.setAttribute('data-musmeta', info);\r\n                        " +
            "block.setAttribute('data-musid', id);\r\n                        " +
            "block.title = item.Title;\r\n                        " +
            "block.innerHTML = ` \r\n                    " +
            "<div class=track__item>  \r\n                        " +
            "<div class=poster__div style=\"background-image: url('${item.Poster}');\">\r\n                            " +
            "<img class=poster__img src=${item.Poster} /></div>\r\n                       " +
            " <div class=track__detail>\r\n                            " +
            "<div class=track__info>\r\n                            " +
            "<div class=track__title> ${item.Title} </div>\r\n                           " +
            "<div class=track__artist>${item.Artist}</div>\r\n                            " +
            "</div>\r\n                            " +
            "<div class=track__duration>${item.Duration}</div>\r\n                        " +
            "</div>\r\n                    " +
            "</div>\r\n        `;\r\n                        " +
            "count++;\r\n                        " +
            "let dl = document.getElementById('telo');\r\n                        " +
            "dl.append(block);\r\n                   " +
            " });\r\n               " +
            " </script>\r\n            " +
            "</ul>\r\n           " +
            " <div class=\"tracks_down\">\r\n                " +
            "<script>\r\n                    " +
            "let back = document.querySelector('div.tracks_down');\r\n                    " +
            "back.innerHTML = 'Всего ' + count;\r\n                " +
            "</script>\r\n            " +
            "</div>\r\n        " +
            "</div>\r\n    " +
            "</div>\r\n    " +
            "<script src=\"https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js\"></script>\r\n    " +
            "<script src=\"https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta1/dist/js/bootstrap.bundle.min.js\"></script>\r\n    " +
            "<script src=\"/js/script.js\"></script>\r\n    <script>\r\n    </script>\r\n</body>\r\n\r\n</html>" +
            "<script type='text/javascript'>" +
            "document.addEventListener(\"DOMContentLoaded\", function (event) {\r\n\r\n            " +
            "const showNavbar = (toggleId, navId, bodyId, headerId) => {\r\n                " +
            "const toggle = document.getElementById(toggleId),\r\n                    " +
            "nav = document.getElementById(navId),\r\n                    " +
            "bodypd = document.getElementById(bodyId),\r\n                    " +
            "headerpd = document.getElementById(headerId)\r\n\r\n                " +
            "// Validate that all variables exist\r\n                " +
            "if (toggle && nav && bodypd && headerpd) {\r\n                    " +
            "toggle.addEventListener('click', () => {\r\n                        " +
            "// show navbar\r\n                        " +
            "nav.classList.toggle('show')\r\n                        " +
            "// change icon\r\n                        " +
            "toggle.classList.toggle('bx-x')\r\n                        " +
            "// add padding to body\r\n                        " +
            "bodypd.classList.toggle('body-pd')\r\n                       " +
            "// add padding to header\r\n                        " +
            "headerpd.classList.toggle('body-pd')\r\n                    " +
            "})\r\n                " +
            "}\r\n            " +
            "}\r\n\r\n            " +
            "showNavbar('header-toggle', 'nav-bar', 'body-pd', 'header')\r\n\r\n            " +
            "/*===== LINK ACTIVE =====*/\r\n            " +
            "const linkColor = document.querySelectorAll('.nav_link')\r\n\r\n           " +
            "function colorLink() {\r\n                " +
            "if (linkColor) {\r\n                    " +
            "linkColor.forEach(l => l.classList.remove('active'))\r\n                   " +
            "this.classList.add('active')\r\n               " +
            "}\r\n           " +
            "}\r\n            " +
            "linkColor.forEach(l => l.addEventListener('click', colorLink))\r\n\r\n            " +
            "// Your code to run since DOM is loaded and ready\r\n        " +
            "});</script>\r\n    " +
            "<script type='text/javascript'>var myLink = document.querySelector('a[href=\"#\"]');\r\n        " +
            "myLink.addEventListener('click', function (e) {\r\n            " +
            "e.preventDefault();\r\n        " +
            "});</script>"
        };
    }
}
