# Réponses aux questions

## Question 10a : Pourquoi on ne voit pas de message de la création dans la page d'index?

Lorsque vous créez une tâche avec l'action Create() GET, vous assignez des valeurs à ViewData, ViewBag et TempData. Après la création réussie d'une tâche, l'action POST Create(CreateTaskVM model) fait un RedirectToAction(nameof(Index)).

ViewData et ViewBag sont perdus après la redirection car ils ne persistent que durant une seule requête HTTP. ViewData et ViewBag sont stockés dans le ViewDataDictionary qui existe uniquement pour la durée d'une requête. Une redirection (RedirectToAction) crée une nouvelle requête HTTP, donc les données sont perdues.

TempData persiste après la redirection et sera visible dans la page Index. TempData utilise un mécanisme de session/cookie qui survit à une redirection jusqu'à ce qu'il soit lu.

## Question 11a : Pourquoi le résultat n'est pas le même?

Dans l'action POST Create(CreateTaskVM model), il y a deux scénarios possibles.

Scénario 1 - Validation échoue (titre vide):
Lorsque la validation échoue, les assignations ViewData/ViewBag/TempData sont faites et la méthode retourne View(model). Cela reste sur la même page. Dans ce cas, ViewData, ViewBag et TempData sont tous visibles dans la vue Create.cshtml.

Scénario 2 - Validation réussit:
Lorsque la validation réussit, la tâche est ajoutée à la liste et la méthode retourne RedirectToAction(nameof(Index)). Cela crée une nouvelle requête HTTP. Dans ce cas, ViewData et ViewBag sont perdus. Seul TempData du GET Create() peut persister vers Index s'il n'a pas été lu.

La raison de cette différence est que return View() reste dans la même requête, tandis que RedirectToAction() crée une nouvelle requête HTTP.

## Question 16a : Quel devrait être le comportement si la catégorie ne correspond à aucune tâche?

L'implémentation actuelle filtre les tâches selon la catégorie fournie. Si la catégorie ne correspond à aucune tâche, la liste tasks sera vide (Count == 0).

Dans ce cas:
- La vue affichera le message "Aucune tâche disponible."
- Le titre affichera "Liste des tâches (Catégorie)"
- Le bouton "Retourner à la liste de tâches complètes" sera affiché

Alternatives possibles:
1. Afficher une liste vide (implémentation actuelle) - Permet de savoir que la catégorie existe mais n'a pas de tâches
2. Rediriger vers Index sans catégorie avec un message d'erreur
3. Retourner NotFound() si la catégorie n'existe pas dans aucune tâche

L'implémentation actuelle est la plus flexible et intuitive pour l'utilisateur.
