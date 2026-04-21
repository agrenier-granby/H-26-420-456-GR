function AppelPartie1() {
    //Appel ajax au serveur
    fetch("/Home/Partie1", {
        method: "POST",
    })
        .then(function (response) {
            if (!response.ok) {
                throw Error(response.status);
            }
            return response;
        })
        .then(function (response) {
            document.querySelector("#reponse1").innerHTML =
                "<p>Appel AJAX avec succes!</p>";
        })
        .catch(function (error) {
            document.querySelector("#reponse1").innerHTML =
                "<p>Echec: " + error + "</p>";
            console.warn(error);
        });
}

function AppelPartie2() {
    //Appel ajax au serveur
    fetch("/Home/Partie2", {
        method: "POST",
    })
        .then(function (response) {
            if (!response.ok) {
                throw Error(response.status);
            }
            return response;
        })
        .then(function (response) {
            document.querySelector("#reponse2").innerHTML =
                "<p>Appel AJAX avec succes!</p>";
        })
        .catch(function (error) {
            document.querySelector("#reponse2").innerHTML =
                "<p>Echec: " + error + "</p>";
            console.warn(error);
        });
}

function AppelPartie3() {
    //Appel ajax au serveur
    console.log(home3)
    fetch(home3, {
        method: "POST",
        body: JSON.stringify({
            Nom: "Charles",
            Age: "20",
            VilleResidence: "Granby",
            Courriel: "cboucher@cegepgranby.qc.ca",
        }),
        headers: {
            "Content-type": "application/json; charset=UTF-8",
        },
    })
        .then(function (response) {
            if (!response.ok) {
                throw Error(response);
            }
            return response.json();
        })
        .then(function (response) {
            document.querySelector("#reponse3").innerHTML =
                "<p>" + JSON.stringify(response, null, 2) + "</p>";
        })
        .catch(function (error) {
            document.querySelector("#reponse3").innerHTML =
                "<p>Echec: " + error.status + "</p>";
            console.warn(error);
        });
}

function AppelPartie4() {
    //Appel ajax au serveur
    fetch("/Home/Partie4", {
        method: "POST",
        body: JSON.stringify({
            Nom: "Charles",
            Age: "20",
            VilleResidence: "Granby",
            Courriel: "cboucher@cegepgranby.qc.ca",
        }),
        headers: {
            "Content-type": "application/json; charset=UTF-8",
        },
    })
        .then(function (response) {
            if (!response.ok) {
                throw Error(response);
            }
            return response.text();
        })
        .then(function (html) {
            document.querySelector("#reponse4").innerHTML = html;
        })
        .catch(function (error) {
            document.querySelector("#reponse4").innerHTML =
                "<p>Echec: " + error + "</p>";
            console.warn(error);
        });
}

function AppelPartie5(ev) {
    ev.preventDefault();
    var csrfToken = document.querySelector(
        'input[name="__RequestVerificationToken"]'
    ).value;

    let client = {
        Nom: "Chuck",
        Age: "31",
        VilleResidence: "Cowansville",
        Courriel: "chuckNorris@universite.qc.ca",
    };
    let client2 = {};
    client2.Nom = "";
    client2.Age = 12;
    client2.VilleResidence = "";
    //Appel ajax au serveur
    fetch("/Home/Partie5", {
        method: "POST",
        body: JSON.stringify(client),
        headers: {
            "Content-type": "application/json; charset=UTF-8",
            RequestVerificationToken: csrfToken,
        },
    })
        .then(function (response) {
            if (!response.ok) {
                throw Error(response);
            }
            return response.text();
        })
        .then(function (html) {
            document.querySelector("#reponse5").innerHTML = html;
        })
        .catch(function (err) {
            document.querySelector("#reponse5").innerHTML =
                "<p>Echec: " + err + "</p>";
            console.warn(err);
        });
}


function AppelPartie6(ev) {
    ev.preventDefault();
    function sendRequestObtenirInfo() {
        fetch("Home/ObtenirVueInfosAleatoire").then(response => {
            if (response.ok) return response.text()
            else throw new Error("Erreur de chargement")
        }).then(html => {
            console.log("Partie 6 HTML: " + html)
            document.querySelector("#partie6_vue").innerHTML = html


            // Get the associated javascript
            fetch('/js/home/obtenirInfosAleatoirePartial.js').then(function (response) {
                if (!response.ok) {
                    return false;
                }
                return response.blob();
            }).then(function (myBlob) {
                var objectURL = URL.createObjectURL(myBlob);
                var sc = document.createElement("script");
                sc.setAttribute("src", objectURL);
                sc.setAttribute("type", "text/javascript");
                document.head.appendChild(sc);
            })


        })
            .catch(error => console.error(error))
    }
    sendRequestObtenirInfo();
}

window.addEventListener("load", () => {
    document.querySelector("#bouton1").addEventListener("click", AppelPartie1);
    document.querySelector("#bouton2").addEventListener("click", AppelPartie2);
    document.querySelector("#bouton3").addEventListener("click", AppelPartie3);
    document.querySelector("#bouton4").addEventListener("click", AppelPartie4);
    document.querySelector("#bouton5").addEventListener("click", AppelPartie5);
    document.querySelector("#bouton6").addEventListener("click", AppelPartie6);
});
