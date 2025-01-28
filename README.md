# 🎮 Projet VR : Multimodal groupe PolyVision
Ce projet est conçue pour fonctionner sur le Meta Quest 3 🥽. Le projet utilise le package **Meta XR All-in-One** pour la gestion des fonctionnalités VR.

### 👨‍💻 Partie de Karim CHARLEUX comprenant :
- (**) Use other non screen based interaction methods (e.g. voice, gesture , motion, etc.) to allow the user to change the atmosphere of the environment, such as the tint and brightness of the lighting
- (**) Implement and demo your application on a VR headset

## ✅ Prérequis
Avant de commencer, assurez-vous d'avoir les éléments suivants installés sur votre machine :
- 🎯 **Unity Hub** avec une version d'Unity supportant le développement VR (2020.3 LTS ou plus récent).
- 📱 **Android Build Support** installé via Unity Hub.
- 🥽 **Meta Quest 3** connecté à votre ordinateur via un câble USB-C ou en mode développeur via Wi-Fi.
- 📦 **Meta XR All-in-One SDK** (il sera inclus dans le package Unity, mais assurez-vous qu'il est bien installé).
---

## 💾 Installation du projet via le Unity Package
1. **Importer le package Unity** 📥 :
   - Ouvrez Unity Hub et créez un nouveau projet Unity (version 2020.3 LTS ou plus récent) ou ouvrez un projet existant.
   - Allez dans **Assets > Import Package > Custom Package**.
   - Sélectionnez le fichier `.unitypackage` 
   - Cliquez sur **Import** pour importer tous les assets, scènes et scripts dans votre projet.

2. **Vérifier les dépendances** 🔍 :
   - Unity devrait importer automatiquement tous les assets et plugins nécessaires, y compris le **Meta XR All-in-One SDK**.
   - Si des erreurs surviennent, assurez-vous que le package **Meta XR All-in-One** est bien installé via le **Package Manager** dans Unity.
---

## ⚙️ Configuration du projet
1. **Configurer les paramètres de build** 🛠️ :
   - Allez dans **File > Build Settings**.
   - Sélectionnez **Android** comme plateforme.
   - Cliquez sur **Switch Platform** pour appliquer les changements.

2. **Configurer les paramètres VR** 🎮 :
   - Allez dans **Edit > Project Settings > XR Plug-in Management**.
   - Cochez **Oculus** sous **Plug-in Providers**.

3. **Configurer le Meta Quest 3** 🔧 :
   - Assurez-vous que votre Meta Quest 3 est en mode développeur. Pour cela, activez le mode développeur dans l'application Oculus sur votre téléphone.
   - Connectez votre Meta Quest 3 à votre ordinateur via un câble USB-C ou en mode Wi-Fi.
---

## 🚀 Exécution du projet
1. **Ouvrir la scène principale** 🎬 :
   - Dans le dossier **Assets/Scenes**, ouvrez la scène **SchoolVR**.

2. **Build et déploiement** 🏗️ :
   - Dans **File > Build Settings**, assurez-vous que la scène **SchoolVR** est ajoutée à la liste des scènes à build.
   - Cliquez sur **Build And Run**. Unity va compiler le projet et le déployer sur votre Meta Quest 3.

3. **Lancer l'application** ▶️ :
   - Une fois le build terminé, l'application devrait se lancer automatiquement sur votre Meta Quest 3.
   - Si ce n'est pas le cas, allez dans **Library > Unknown Sources** sur votre Meta Quest 3 et lancez l'application manuellement.
---
