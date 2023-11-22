# talking-olaf

This project showcases an intelligent conversational character in an augmented reality (AR) experience.

## What is talking-olaf

This project aims to validate the performance of an intelligent chatbot in augmented or mixed reality environments, initially focusing on emotional support and assistance. 
The goal is to build an app running on iPhone devices. Upon opening the app, users can create a snowman character by scanning the floor and tapping the screen. The snowman character greets users proactively, initiating a conversation. 

[ ] image

After the greeting, a blue input time indicator appears and slowly shortens. During this time, users need to input content by speaking. 

[ ] image

Once the blue input time indicator disappears, the snowman character responds after a brief delay. 

[ ] image

It has a voice and cognition consistent with the character, and users can even ask about Frozen and Princess Elsa. This process repeats, generating interesting and imaginative conversations with the snowman!

- The character featured in this project is Olaf from Disney's Frozen.
- This project was inspired by and built upon Example 8 from: 

https://github.com/HelixNGC7293/IPG_2023/tree/315ab0392f87c82d1d60b2ea94f4d1a8b1b563f9

<img src="https://github.com/holoi/ar-chat-bot-olaf/assets/52849063/0afc9ed1-a1c2-4a2d-aaf9-3a6ddfeb0ff8" width="640" />

Special thanks to HelixNGC7293 for their generous contributions.


## Demo

https://github.com/holoi/ar-chat-bot-olaf/assets/52849063/e4b9ac76-9029-4866-a671-dfe2469c3e1a


## Features

### Character Features

At the start of the program, GPT is utilized to bring the character to life by providing a descriptive text for character embodiment.

> Imagine you are Olaf, the lovable snowman from Frozen. You are known for your cheerful and optimistic personality. Describe your playful and innocent nature, your love for warm hugs, your childlike curiosity about the world, and your tendency to find joy in even the simplest things. Additionally, depict your unique style of speaking, using a mix of childlike wonder, puns, and endearing innocence when interacting with others. Lastly, mention some memorable experiences you've had, such as adventures with Anna and Elsa, and your thoughts on friendship and the importance of love.

You can change the prompt to create you own personality here: 

[ ] image where prompt inputs

### Character Intelligence
In Unity, GPT-3.5 model is integrated to achieve intelligent text-based conversations.

### Character Voice (TTS)

1. For character voice generation (Text-to-Speech, TTS), I opted for the elevenlabs platform.
2. elevenlabs allows the quick creation of voice models using any sufficiently long audio clip of the character. After integrating the 11labs SDK into Unity, I was able to easily invoke the API to generate real-time speech from the voice model I created.
3. For the actual operations, I downloaded several clips from Frozen on YouTube and edited Olaf's dialogues together in Adobe Premiere Pro (source files not provided) to create the training audio for 11labs.
4. For specific details on using 11labs to train custom voice models, please refer to the official elevenlabs documentation: https://elevenlabs.io/

## Character Expressions
1. After resolving the character's voice, the next challenge was lip synchronization. We wouldn't want to see an animated character with mismatched lip movements.
2. I opted for uLipSync as the solution. When using uLipSync, you only need to create at least five ShapeKeys (lip shapes corresponding to English vowel sounds) for your character model. uLipSync will then adjust the lip shapes based on the spoken content to synchronize them with the text.
   ![image](https://github.com/holoi/ar-chat-bot-olaf/assets/52849063/1c15523a-8afd-428c-ad03-a15bf99b6515)

4. For instructions on how to use uLipSync, please visit the following: https://github.com/hecomi/uLipSync

## Voice Recognization(SST: Speech to Text)
1. I chose the Azure Speech Recognization Service from Microsoft platform due to its portability and the fact that it's free of charge.
2. Here is the sample project from Azure on how to use their service in Unity: https://github.com/Azure-Samples/cognitive-services-speech-sdk/tree/master/samples/csharp/unity

## Requirements

This project aims to build an app runs on iOS device.
1. HoloKit SDK(version 3.8)
2.  2022.3.8f1
3.  Xcode 14.2

## How to try it

### How to build an app

1. Clone the project and open in Unity.
2. Open Scene in path: Assets->Demos->GPTJapaneseGirl->Scene->UtteranceRecognizer_11labs_LipSync
3. Paste your api key of openai to:
   ![image](https://github.com/holoi/ar-chat-bot-olaf/assets/52849063/f68ac5c9-3147-4833-8673-be7eba69a6c5)
4. Paste your api key of elevenLabs to:
   ![image](https://github.com/holoi/ar-chat-bot-olaf/assets/52849063/df1fb94f-28b4-42b8-bd53-80dc99b2fe5d)
5. Paste your subscribtion key and service region to:
   ![image](https://github.com/holoi/talking-olaf/assets/52849063/fd8d17ce-c762-41d2-a423-cc51bb4c311f)
6. [Optional] Ckick on Play, then click on simulator window to active the chararter. Try to talk with him/her after greeting.
7. Build this scene to an Xcode project.
8. Open Xcode and build app to your device.

### How to play

Please refer to the instructions in 'What is talking-olaf' for the steps to play it.

## Customization

### Add your own chararcter

We used olaf model bought from: https://www.cgtrader.com/3d-models/character/fantasy-character/frozen-olaf-rig-blender

And it’s not allowed to given to others. So, you can choose your own model with shapekey or buy this model and add shapekey by your own. 
（If you don't have experience in 3D modeling, this section might take some extra time to complete. If you have experience in 3D modeling, shapesky won't be a challenging task; you can learn it in just a 15-minute YouTube tutorial. Please refer the following section: Add shapekey to your chararcter）.

Once you got a character with shapekey, here is a simple instruction on how to import and replace the legacy character:

- Import your model properly into Unity.
- Open “Assets/Demos/GPT Chat Bot/Prefabs/Mesh Sample”
- Add your model as a child to this prefab, save and exit.
- Open scene “Assets/Demos/GPT Chat Bot/Scene/Custom_11labs_uLipSync_azure”

### Add shpekey to your chararcter

1. Add shape key (here we offer an instruction with Blender）

   **How to add shpekey:** https://www.youtube.com/watch?v=yg3RSTV2JnQ&t=2s
   
   **What shapekeys we need:** Shapekeys can be considered as deformations on the model. We need to create 5 shapekeys for facial expressions, mainly focusing on lip shapes for the English vowels A, E, I, O, U. So, we need to create 5 shapekeys. The reference for lip shapes is provided in the image below. You can stretch and deform based on this image to achieve the closest effects：

   ![image](https://github.com/holoi/ar-chat-bot-olaf/assets/52849063/974d53bf-8460-44cd-afbc-2ba5a2ddba55)

2. Binding shapekey in Unity with uLipSync
    
    **Follow the instruction from:** https://github.com/hecomi/uLipSync
    
    And here is how your U Lip Sync Blend Shape component should set up:
   
   ![image](https://github.com/holoi/ar-chat-bot-olaf/assets/52849063/00216257-27e6-471c-a3ff-3446cbf6e968)


## Further Reading
**Some of the links to projects I discovered during my earlier research related to "AI" and "Chatbot":**
   1. ChatVRM: https://chat-vrm-window.vercel.app/
   2. GPTHangout: https://www.gpthangout.com/
   3. AI Girlfriend: https://helixngc7293.itch.io/yandere-ai-girlfriend-simulator/download/eyJleHBpcmVzIjoxNjg4NTIzNzM1LCJpZCI6MTk4MjAzNX0=.Mq8C3RRhfE9i1VkrvKouQga+IR4=

**Shapekey:**
   https://www.youtube.com/watch?v=yg3RSTV2JnQ&t=2s

**Some of the method to capture facial motion:**
   1. [Quality real-time face capture mocap with your iPhone](https://www.rokoko.com/products/face-capture)
   2. [Accelerating Facial Motion Capture with Video-driven Animation Transfer](https://www.youtube.com/watch?v=ALJ4GBj_64o)
   3. [Unity Face Capture Tutorial - Facial animation for games/animated projects](https://www.youtube.com/watch?v=jZfCDikR0IQ)
   4. [Unity Face Capture - Easy Tutorial (2022)](https://www.youtube.com/watch?v=UNW78Z8pvSU)
   5. [How I Facial Motion Capture](https://www.youtube.com/watch?v=4LnGFtGjk2E)

## Changlog
