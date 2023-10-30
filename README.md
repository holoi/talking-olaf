# talking-olaf

# Changlog

# Abstract
This project showcases an intelligent conversational character in an augmented reality (AR) experience. The character featured in this project is Olaf from Disney's Frozen.
This project was inspired by and built upon Example 8 from: https://github.com/HelixNGC7293/IPG_2023/tree/315ab0392f87c82d1d60b2ea94f4d1a8b1b563f9, special thanks to HelixNGC7293 for their generous contributions.
![image](https://github.com/holoi/ar-chat-bot-olaf/assets/52849063/0afc9ed1-a1c2-4a2d-aaf9-3a6ddfeb0ff8)


# Demo
https://github.com/holoi/ar-chat-bot-olaf/assets/52849063/e4b9ac76-9029-4866-a671-dfe2469c3e1a


# Features
## Character Features
At the start of the program, GPT is utilized to bring the character to life by providing a descriptive text for character embodiment.
> Imagine you are Olaf, the lovable snowman from Frozen. You are known for your cheerful and optimistic personality. Describe your playful and innocent nature, your love for warm hugs, your childlike curiosity about the world, and your tendency to find joy in even the simplest things. Additionally, depict your unique style of speaking, using a mix of childlike wonder, puns, and endearing innocence when interacting with others. Lastly, mention some memorable experiences you've had, such as adventures with Anna and Elsa, and your thoughts on friendship and the importance of love.

## Character Intelligence
In Unity, GPT-3.5 model is integrated to achieve intelligent text-based conversations.

## Character Voice (TTS)
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

# System requirements

Unity 2022.3.8f1

# How to try it
Fist, clone the project and open in Unity.
## Add your own model with shapekey
We used olaf model bought from: https://www.cgtrader.com/3d-models/character/fantasy-character/frozen-olaf-rig-blender

And it’s not allowed to given to others. So, you can choose your own model with shapekey or buy this model and add shapekey by your own.

- Import your model properly into Unity.
- Open “Assets/Demos/GPT Chat Bot/Prefabs/Mesh Sample”
- Add your model as a child to this prefab, save and exit.
- Open scene “Assets/Demos/GPT Chat Bot/Scene/Custom_11labs_uLipSync_azure”

## If your model does not have proper shapekey
1. Add shape key to it(in Blender)
    Here is a simple tutorial on how to add shpekey in Blender: https://www.youtube.com/watch?v=yg3RSTV2JnQ&t=2s
   
   We need 5 shape key:
   
   - mouth shape A
   - mouth shape E
   - mouth shape I
   - mouth shape O
   - mouth shape U

   ![image](https://github.com/holoi/ar-chat-bot-olaf/assets/52849063/974d53bf-8460-44cd-afbc-2ba5a2ddba55)

2. Binding shapekey with uLipSync
    
    Follow the instruction from: https://github.com/hecomi/uLipSync
    
    Here is how your U Lip Sync Blend Shape component should be:
   ![image](https://github.com/holoi/ar-chat-bot-olaf/assets/52849063/00216257-27e6-471c-a3ff-3446cbf6e968)
## Run the project
1. Paste your api of openai to API KEY
   ![image](https://github.com/holoi/ar-chat-bot-olaf/assets/52849063/f68ac5c9-3147-4833-8673-be7eba69a6c5)
2. Paste your api of elevenLabs to Api Key
   ![image](https://github.com/holoi/ar-chat-bot-olaf/assets/52849063/df1fb94f-28b4-42b8-bd53-80dc99b2fe5d)
3. Play, click on simulator window to active your model(character). Wait him/her says an introducing first and try to talk with him/her.
4. Build to a Xcode project and build in Xcode to have an AR Experience or StereoAR Experience with HoloKit X.


# Further Reading
Some of the links to projects I discovered during my earlier research related to "AI" and "Chatbot":
   1. ChatVRM: https://chat-vrm-window.vercel.app/
   2. GPTHangout: https://www.gpthangout.com/
   3. AI Girlfriend: https://helixngc7293.itch.io/yandere-ai-girlfriend-simulator/download/eyJleHBpcmVzIjoxNjg4NTIzNzM1LCJpZCI6MTk4MjAzNX0=.Mq8C3RRhfE9i1VkrvKouQga+IR4=

Shapekey:
   https://www.youtube.com/watch?v=yg3RSTV2JnQ&t=2s

Some of the method to capture facial motion:
   1. [Quality real-time face capture mocap with your iPhone](https://www.rokoko.com/products/face-capture)
   2. [Accelerating Facial Motion Capture with Video-driven Animation Transfer](https://www.youtube.com/watch?v=ALJ4GBj_64o)
   3. [Unity Face Capture Tutorial - Facial animation for games/animated projects](https://www.youtube.com/watch?v=jZfCDikR0IQ)
   4. [Unity Face Capture - Easy Tutorial (2022)](https://www.youtube.com/watch?v=UNW78Z8pvSU)
   5. [How I Facial Motion Capture](https://www.youtube.com/watch?v=4LnGFtGjk2E)
