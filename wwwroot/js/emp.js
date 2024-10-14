const inputGroupDept = document.getElementById("inputGroupDept");
let inputGroupPoste = document.getElementById("inputGroupPoste");

if (inputGroupDept != null && inputGroupPoste) {
    inputGroupDept.onchange = () => {
        var idDept = inputGroupDept.value;
        $.ajax({
            url: '/Dept/getAllPostes',
            type: 'GET',
            data: { idDept: idDept },
            success: function (postes) {
                console.log(postes);
                postes.forEach(function (poste) {
                    var option = document.createElement("option");
                    option.value = poste.id;
                    option.text = poste.nom;
                    inputGroupPoste.appendChild(option);
                });
            },
            error: function () {
                console.log("Erreur lors de la recuperation des postes");
            }
        });
    };
}