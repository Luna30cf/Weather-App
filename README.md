# <center> Weather App

![logo](Assets/logo.svg)

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

    - Le nom de la ville

    - La température en degré Celsius

    - Courte description du temps

    - La latitude et la longitude de la ville

    - L’humidité


* **Prévision :**

    - 5 colonnes

    - Les prévisions météorologiques devront être à 12:00

    - La date de la prévision et l’heure devront être affiché


* **Paramètres :**

    - Ville par défaut

    - changement de la langue


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

* *[Assets](Assets/):*
    * icônes faites à la main
    * exporter via figma donc format svg

### 2 - Installation

Pour cloner le répository:

```
https://github.com/Luna30cf/Weather-App.git
```


Pour créer les fichiers.json:

- options.json
```
{
    "lang": "langue choisie (fr pour français, en pour anglais etc)",
    "defaultCity": "ville choisie"
  }
```
- config.json
```
{
    "ApiKey": "votre clé API"
  }
```


Pour installer le package NuGet `Newtonsoft.Json` (à la racine):

```
Weather-App> dotnet add package Newtonsoft.Json
```

### 3 - Lancement


Pour lancer le projet (à la racine): 

```
Weather-App> dotnet run
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