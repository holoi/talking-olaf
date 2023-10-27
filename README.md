![image](https://github.com/holoi/ar-chat-bot-olaf/assets/52849063/5753a5e3-bbd7-4d68-b25a-26d06e7be131)# ar-chat-bot-olaf
This project is a unity project, creating an ar chat bot and have a conversation! 

# Project Description:
This project showcases an intelligent conversational character in an augmented reality (AR) experience. The character featured in this project is Olaf from Disney's Frozen.
This project is an extension of Example 8 from: https://github.com/HelixNGC7293/IPG_2023/tree/315ab0392f87c82d1d60b2ea94f4d1a8b1b563f9. Special thanks to HelixNGC7293 for their generous contributions.
![image](https://github.com/holoi/ar-chat-bot-olaf/assets/52849063/0afc9ed1-a1c2-4a2d-aaf9-3a6ddfeb0ff8)

# Demo

# Technical Description
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

## SST(Speech to Text)
1. I chose the Azure Speech Recognization Service from Microsoft platform due to its portability and the fact that it's free of charge.
2. Here is the sample project from Azure on how to use their service in Unity: https://github.com/Azure-Samples/cognitive-services-speech-sdk/tree/master/samples/csharp/unity

## Further Reading
Here, I will provide some of the links I discovered during my earlier research related to "AI" and "Chatbot".
ChatVRM: https://chat-vrm-window.vercel.app/
GPTHangout: https://www.gpthangout.com/
AI Girlfriend: https://helixngc7293.itch.io/yandere-ai-girlfriend-simulator/download/eyJleHBpcmVzIjoxNjg4NTIzNzM1LCJpZCI6MTk4MjAzNX0=.Mq8C3RRhfE9i1VkrvKouQga+IR4=
