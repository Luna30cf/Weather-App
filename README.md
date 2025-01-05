# <center> Weather App

## Sommaire

- [I - Fonctionnement](#i---fonctionnement)
    - [1 - Explications](#1---explications)
        - [a - Consigne](#a---consigne)
        - [b - Arborescence](#b---arborescence)
    - [2 - Installation](#2---installation)
    - [3 - Lancement](#3---lancement)
- [II - Répartition](#ii---répartition)
- [III - Crédits](#iii---crédits)



## I - Fonctionnement

### 1 - Explications

#### a - Consigne

* **Recherche par ville :**

    -  Le nom de la ville

    - La température en degré Celsius

    - Courte description du temps


* **Prévision :**


* **Paramètres :**


#### b - Arborescence

* *[Models](Models/):*  
    * données et logique métier,   
    * gestion des méthodes et des propriétés   
    * dépend de l'interface utilisateur

* *[ViewModels](ViewModels/):*   
    * lien entre Views (`*.axaml`) et Models(`*.cs`), donc `*.axaml.cs`
    * logique pour préparer et gérer les données affichées dans la vue.      
    * Commandes (répondre aux interactions utilisateurs)

* *[Views](Views/):*
    * interface utilisateur
    * gestion de l'affichage des données et de l'interaction avec l'utilisateur.
    * Parallèle avec html/css possible

### 2 - Installation

Pour installer le package NuGet `Newtonsoft.Json` (à la racine):

```
WeatherApp> dotnet add package Newtonsoft.Json
```

### 3 - Lancement


Pour lancer le projet (à la racine): 

```
WeatherApp> dotnet run
```


## II - Répartition

### Karl

* *mise en place de la base du projet*
* *installation d'avalonia*   


### Luna

* *Récupération de la clé API* 
* *structuration du projet avec l'arborescence MVVM*
* *Readme*   


### Erika

* *Recherche par ville*
* *Powerpoint*

   
## III - Crédits

| Noms                         |                Adresses Mails |
| :-----------------------     |      -----------------------: |
|**Luna COLOMBAN-FERNANDEZ**   |luna.colombanfernandez@ynov.com|
|**Erika LAJUS**               |erika.lajus@ynov.com           |
|**Karl DAVAL-LECLERCQ**       |karl.davalleclercq@ynov.com    |