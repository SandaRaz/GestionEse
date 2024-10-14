const articleChoisit = document.getElementById("articleChoisit");
let unitesArticle = document.getElementById("unitesArticle");
let unitesPuArticle = document.getElementById("unitesPuArticle");

const idArticleInput = document.getElementById("idArticleInput");
if (idArticleInput != null) {
    window.onload = () => {
        console.log("Window loaded");
        var idArticle = idArticleInput.value;
        $.ajax({
            url: '/Service/GetUnites',
            type: 'GET',
            data: { idarticle: idArticle },
            success: function (unites) {
                console.log(unites);
                if (unitesArticle != null) {
                    unitesArticle.innerHTML = "";
                    unites.forEach(function (unite) {
                        let option = document.createElement("option");
                        option.value = unite.id;
                        option.text = unite.unite + " (" + unite.id + ")";
                        unitesArticle.appendChild(option);
                    });
                }
            },
            error: function () {
                console.log("Erreur lors de la recuperation de la liste d'unites");
            }
        });
    }
}

if (articleChoisit != null) {
    articleChoisit.onclick = () => {
        var idArticle = articleChoisit.value;
        $.ajax({
            url: '/Service/GetUnites',
            type: 'GET',
            data: { idarticle: idArticle },
            success: function (unites) {
                console.log(unites);
                if (unitesArticle != null) {
                    unitesArticle.innerHTML = "";
                    unites.forEach(function (unite) {
                        let option = document.createElement("option");
                        option.value = unite.id;
                        option.text = unite.unite + " (" + unite.id + ")";
                        unitesArticle.appendChild(option);
                    });
                }
                if (unitesPuArticle != null) {
                    unitesPuArticle.innerHTML = "";
                    unites.forEach(function (unite) {
                        let option = document.createElement("option");
                        option.value = unite.id;
                        option.text = unite.unite + " (" + unite.id + ")";
                        unitesPuArticle.appendChild(option);
                    });
                }
            },
            error: function () {
                console.log("Erreur lors de la recuperation de la liste d'unites");
            }
        });
    }
}