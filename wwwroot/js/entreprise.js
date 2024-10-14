let mdpEse = document.getElementById('mdp');
let confirmMdpEse = document.getElementById('confirmmdp');

if (confirmMdpEse != null && mdpEse != null) {
    confirmMdpEse.addEventListener('keyup', () => {
        if (mdpEse.value.length > 0) {
            if (mdpEse.value != confirmMdpEse.value) {
                confirmMdpEse.style.color = 'red';
            } else {
                confirmMdpEse.style.color = 'grey';
            }
        }
    });
}

const fetchIdPoste = (idDeptChoisit, idPostChoix) => {
    var idDept = idDeptChoisit.value;
    $.ajax({
        url: '/Service/GetPosts',
        type: 'GET',
        data: { idDept: idDept },
        success: function (posts) {
            console.log(posts);
            if (idPostChoix != null) {
                idPostChoix.innerHTML = '';
                posts.forEach(function (post) {
                    let option = document.createElement('option');
                    option.value = post.id;
                    option.text = post.nom;
                    idPostChoix.appendChild(option);
                });
            }
        },
        error: function () {
            console.log('Erreur lors de la recuperation de la liste de postes');
        }
    });
};

const idDeptChoisit = document.getElementById('idDeptChoisit');
let idPostChoix = document.getElementById('idPostChoix');
if (idDeptChoisit != null) {
    document.addEventListener('DOMContentLoaded', fetchIdPoste(idDeptChoisit, idPostChoix));
    window.addEventListener('beforeunload', fetchIdPoste(idDeptChoisit, idPostChoix));
    idDeptChoisit.onclick = () => {
        fetchIdPoste(idDeptChoisit, idPostChoix);
    }
}

