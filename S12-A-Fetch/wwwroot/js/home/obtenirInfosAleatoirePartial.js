function sendRequestObtenirInfo(ev) {
    ev.preventDefault();
    fetch("Home/ObtenirInfosAleatoire",
        {
            method: "POST"
        }).then(response => {
            if (response.ok) return response.text()
            else throw new Error("Erreur de chargement")
        }).then(html => { document.querySelector("#obtenirInfosPartialResultat").innerHTML = html })
        .catch(error => console.error(error))
}
console.log("obtenirInfosAleatoirePartial.js loaded")
document.querySelector("#obtenirInfoBtn").addEventListener("click", sendRequestObtenirInfo);