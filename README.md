# 🎮 Projet VR : Multimodal groupe PolyVision
Ce projet est conçu pour fonctionner sur le Meta Quest 3 🥽. Le projet utilise le package **Meta XR All-in-One** pour la gestion des fonctionnalités VR.

### 👨‍💻 Partie de Karim CHARLEUX comprenant :
- (**) Use other non screen based interaction methods (e.g. voice, gesture , motion, etc.) to allow the user to change the atmosphere of the environment, such as the tint and brightness of the lighting
- (**) Implement and demo your application on a VR headset

---

## ✅ Prérequis
Avant de commencer, assurez-vous d'avoir les éléments suivants installés sur votre machine :
- 🎯 **Unity Hub** avec une version d'Unity supportant le développement VR (2022.3 LTS ou plus récent).
- 📱 **Android Build Support** installé via Unity Hub.
- 🥽 **Meta Quest** connecté à votre ordinateur via un câble USB-C ou en mode développeur via Wi-Fi.

---

## 💾 Téléchargement et importation du projet
1. **Télécharger le projet** 📥 :
   - Clonez ou téléchargez le dépôt GitHub à l'adresse suivante : [https://github.com/KarimCharleux/multimodal-vr-metaquest](https://github.com/KarimCharleux/multimodal-vr-metaquest).

2. **Ouvrir le projet dans Unity** 🎮 :
   - Ouvrez Unity Hub, cliquez sur **Add**, puis **Add project from disk** et sélectionnez le dossier du projet cloné/téléchargé.
   - Lancer le projet importé
   - Unity importe automatiquement tous les assets, dépendances, scènes et scripts.

3. **Vérifier les dépendances** 🔍 :
   - Unity devrait importer automatiquement tous les assets et plugins nécessaires, y compris le **Meta XR All-in-One SDK**.
   - Si des erreurs surviennent, assurez-vous que le package **Meta XR All-in-One** est bien installé via le **Package Manager** dans Unity.

---

## ⚙️ Configuration du projet
1. **Configurer les paramètres de build** 🛠️ :
   - Allez dans **File > Build Settings**.
   - Sélectionnez **Android** comme plateforme.
   - Cliquez sur **Switch Platform** pour appliquer les changements.

2. **Configurer le Meta Quest 3** 🔧 :
   - Assurez-vous que votre Meta Quest est en mode développeur. Pour cela, activez le mode développeur dans l'application Oculus sur votre téléphone.
   - Connectez votre Meta Quest à votre ordinateur via un câble USB-C ou en mode Wi-Fi.

---

## 🚀 Exécution du jeu
1. **Build et déploiement** 🏗️ :
   - Dans **File > Build Settings** cliquez sur la liste des **Run device** et selectionnez **Oculus Quest X**.
   - Assurez-vous que la scène **SchoolVR** est ajoutée et cochée dans la liste des scènes à build.
   - Cliquez sur **Build And Run**. Unity va compiler le projet et le déployer sur votre Meta Quest.

3. **Lancer l'application** ▶️ :
   - Une fois le build terminé, l'application devrait se lancer automatiquement sur votre Meta Quest.
   - Si ce n'est pas le cas, allez dans le menu **Library > Unknown Sources** sur votre Meta Quest 3 et lancez l'application manuellement.
   - Une fois l'application lancée, autoriser l'accès au micro via la fenètre qui s'ouvrira.
   - Dans le cas où vous avez la musique mais pas l'image, fermez le jeu et le lancez le manuellement dans le menu **Library > Unknown Sources** sur votre Meta Quest.


4. **Guide de jeu** 🎮 :
   - **Déplacement** 🕹️ :
      - Utilisez les joysticks des manettes pour vous déplacer.
      - Vous pouvez également vous déplacer en utilisant vos mains.
   - **Interaction avec les objets** 🖐️ :
      - Prenez les tables présentes dans les classes en utilisant les boutons des manettes (appuyez avec le majeur de votre main).
      - Vous pouvez également attraper les objets en pinçant votre pouce et votre index pour les manipuler.
   - **Changer la météo et l'horaire** 🌦️ :
      - Cliquez sur un des boutons pour activer la commande vocale. Un texte "Listening..." apparaît à l'écran.
      - Prononcez une phrase pour changer la météo ou l'horaire. Quatre modes sont disponibles :
         - **Météo** : "pluie", "soleil".
         - **Horaire** : "nuit", "jour".
      - Si le système comprend votre commande, l'environnement se met à jour automatiquement.

---

### 🖇️ Liens vers les ressources Unity utilisées :
- https://assetstore.unity.com/packages/2d/textures-materials/sky/free-stylized-skybox-212257
- https://assetstore.unity.com/packages/2d/textures-materials/pack-free-textures-2-266006
- https://assetstore.unity.com/packages/3d/environments/school-assets-146253
- https://assetstore.unity.com/packages/tools/integration/meta-xr-all-in-one-sdk-269657
