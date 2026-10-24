using Godot;
using System;
using System.Collections.Generic;

public partial class Sala1 : Node2D
{
    GameState gameState;
    AudioStreamPlayer2D musica;
    AudioStreamPlayer2D somDialogo;
    Action aoTerminarFala;
    Tween tweenTexto;
    bool livroLido = false;
    bool licaoLiberada = false;
    bool remedioTomado = false;
    bool historiaContada = false;
    Button botaoJogarLixo;
    Node2D[] comodos;
    private CanvasLayer painelTarefa;

    // Falas organizadas por dia e local. Linhas iniciadas com # são comandos internos.
    Dictionary<(int dia, string local), string[]> falas = new()
    {
        // DIA 1
        { (1, "Espelho"), new[] {
            "Cherry|Good morning, Cherry.", "Cherry|All right... what do we have planned for today?",
            "Cherry|Let's see... lunch, getting Sugarplum ready for school, folding the laundry...",
            "Cherry|His first day of school. I wonder if he's excited.", "Cherry|He's growing up so fast.",
            "Cherry|Well, I should get started. No time to waste!"
        }},
        { (1, "Cozinha"), new[] { "Cherry|All right, let's get everything ready.", "Cherry|I should prepare lunch first." }},
        { (1, "AlmocoPronto"), new[] {
            "Cherry finishes cooking and leaves the backpack by the entrance.",
            "Cherry|Well, that wasn't so hard.",
            "Cherry|Everything's ready, and right on time.",
            "Cherry|Now, let's get my little Sugarplum.",
            "Cherry|Sugarplum! Lunch is ready!"
        }},
        { (1, "Mesa"), new[] {
            "Cherry serves Sugarplum and sits down with him.",
            "Cherry|Careful, it's still hot.", "Sugarplum|It smells so good!",
            "Cherry|Well, you'd better eat it before it gets cold.", "Sugarplum|Can I have more if I want some?",
            "Cherry|Of course, silly. Just eat what you've got first.",
            "Cherry|So... first day of school. Nervous?", "Sugarplum|A little.", "Cherry|Yeah? What about?",
            "Sugarplum|I don't know. What if nobody wants to be my friend?", "Cherry|I'm sure you'll find someone. You just have to talk to them.",
            "Sugarplum|But what if I don't know what to say?", "Cherry|Then... ask them what they like. Or if they wanna play something. That's what I used to do.",
            "Sugarplum|You did?", "Cherry|Of course! You think I was born knowing how to talk to people?", "Sugarplum giggles.",
            "Sugarplum|Kinda.", "Cherry|Oh, you little rascal.", "They continue eating.",
            "Sugarplum|Mommy?", "Cherry|Hmm?", "Sugarplum|You'll be here when I get back, right?",
            "Cherry|Of course, sweetheart. Where else would I be?", "Sugarplum|Okay.",
            "After lunch, Cherry clears the table while Sugarplum grabs his backpack.",
            "Sugarplum|I'm ready!", "Cherry|Already? Let me see your backpack.", "Cherry checks his school supplies and adjusts the straps.",
            "Cherry|Yep, looks like you've got everything.", "Sugarplum|Can we go now?", "Cherry|Hold on, let me grab my things. No need to rush.",
            "Cherry|All right, let's go.", "Sugarplum|Yay!"
        }},
        { (1, "SalaComFilho"), new[] {
            "Cherry adjusts Sugarplum's backpack.", "Cherry|There. Is it too heavy?", "Sugarplum|Nope!",
            "Cherry|Good. Did you get everything?", "Sugarplum|I think so.", "Cherry|You think so?",
            "Cherry quickly checks his backpack.", "Cherry|Looks fine to me.",
            "Sugarplum|Bye, Mommy!", "Cherry|Bye, sweetheart. Have a good day!", "Sugarplum|I will!",
            "Sugarplum walks away. Cherry watches from the doorway.", "Cherry|And don't forget to eat your lunch!",
            "Sugarplum|I won't!", "#sala", "Cherry closes the door. The apartment falls silent."
        }},
        { (1, "fundosala"), new[] {
            "Cherry|Well, that's that.",
            "She notices the toys scattered around the bedroom.",
            "Cherry|I swear, he was tiny just yesterday.",
            "Cherry|First day of school already...",
            "Cherry checks the time.",
            "Cherry|I've got some time before dinner.",
            "She picks up her book."
        }},
        { (1, "Livro"), new[] {
            "Cherry sits on the couch and reads Hamnet, the book her mother recommended.",
            "Cherry|Okay, where was I?", "She finds her page and continues reading.",
            "Cherry|Mom was right. This is actually pretty good.", "Cherry|I should tell her I'm enjoying it.",
            "Cherry picks up her phone, looks at it, then puts it down.", "Cherry|It's been a while since she came over.",
            "She glances toward Sugarplum's toys.", "Cherry|Sugarplum would love to see her.",
            "Cherry considers sending a message but returns to her book.", "Cherry|Maybe I'll call her later.",
            "She closes the book and notices the laundry basket.", "Cherry|Oh, right. The laundry.",
            "Cherry places the basket on the couch and starts folding the clothes.",
            "Cherry|How does such a little boy make so much laundry?", "Cherry|At least these are clean."
        }},
        { (1, "PosDobrarRoupa"), new[] {
            "Cherry|There. Much better.",
            "She takes the clothes to the bedroom and spots a toy near the bed.",
            "Cherry|I really need to get him to put his clothes away.",
            "She puts the toy with the others.",
            "Cherry|And these toys... I'll leave those for later.",
            "Cherry leaves the room.",
            "Cherry|All right. Dinner next."
        }},
        { (1, "Licao"), new[] { "Cherry sits beside Sugarplum as he opens his notebook.", "Sugarplum|Here it is!", "Cherry|Let's see... Oh, that's not too bad.", "Sugarplum|Can you help me?", "Cherry|Of course. Let's do it together." }},
        { (1, "PosLicao"), new[] { "Cherry closes the notebook. Sugarplum starts putting his pencils away.", "Sugarplum|Can I go play now?", "Cherry|Sure, just put your things away first.", "Sugarplum|Okay!", "Cherry watches him put his things away.", "Cherry|There. One more thing done." }},
        { (1, "Remedio"), new[] { "Cherry finishes tidying the kitchen and looks at herself in the bathroom mirror.", "Cherry|What a day.", "She finishes her nighttime routine and returns to the bedroom." }},
        { (1, "HistoriaUrsos"), new[] {
            "Sugarplum is already in bed, surrounded by toys and drawings.",
            "Sugarplum|Mommy! You're back!", "Cherry|I told you I'd be right back.", "Sugarplum|Can you tell me a story?", "Cherry|The bear story?", "Sugarplum|The bear story!", "Cherry|All right, get comfy.",
            "Cherry opens the book and begins reading.",
            "Cherry|Once upon a time, there was a mother bear named Berry. She loved wandering through the forest, picking little berries. Blueberries, blackberries, raspberries... she loved all kinds of berries.",
            "Cherry|Berry always took her little cub with her to pick berries. But unlike Berry, her cub preferred other fruits, like apples, apricots, and his favorite of all, plums.",
            "Cherry|He loved plums so much that his mother gave him the nickname Little Plum. And whenever they went out to pick fruit, she always made sure to find at least one plum to bring back to her little cub.",
            "Cherry smiles and closes the book for the night.", "Cherry|We'll read the rest tomorrow, okay?", "Sugarplum|Okay. Goodnight, Mommy.", "Cherry|Goodnight, my little Sugarplum. I love you.", "Sugarplum|Love you too.",
            "End of Day 1."
        }},
        { (1, "PrepararJantar"), new[] { "Cherry|All right, dinner time.", "Cherry|Let's see what we've got...", "Cherry gathers the ingredients.", "Cherry|This should do." }},
        { (1, "JantarPronto"), new[] {
            "Cherry finishes cooking and sets the plates.",
            "Cherry|I hope he's hungry.", "Cherry|There we go.", "Cherry|Dinner's ready!"
        }},
        { (1, "MesaJantar"), new[] {
            "Cherry serves Sugarplum.",
            "Cherry|There you go. Careful, it's hot.", "Sugarplum|I know, I know!",
            "Sugarplum takes a bite.",
            "Sugarplum|Mmm! This is good!", "Cherry|Glad you like it.", "Sugarplum|Can I have more?", "Cherry|You haven't even finished that yet!", "Sugarplum|But I'm hungry!", "Cherry|All right, all right. Finish that first, and I'll get you some more." }},

        // DIA 2
        { (2, "Acordar"), new[] {
            "Cherry|Morning, Cherry.",
            "Cherry|Let's see... lunch, taking out the trash, dinner...",
            "Cherry|Another day of school for Sugarplum.",
            "#quarto",
            "Cherry|I hope he had a good time yesterday.",
            "Cherry|Sugarplum, time to wake up.", "Sugarplum|Already?", "Cherry|Yep. Come on, sleepyhead.",
            "Sugarplum, still sleepy, gets out of bed."
        }},
        { (2, "Cozinha"), new[] { "Cherry|Let's see what I can make today.", "Cherry|Sugarplum could use a little more variety." }},
        { (2, "AlmocoPronto"), new[] { "Cherry arranges the table.", "Cherry|Lunch is ready!" }},
        { (2, "MesaAlmoco"), new[] {
            "Cherry|Did you sleep well, honey?", "Sugarplum|Yeah. Can we have dessert today?",
            "Cherry|You haven't even finished your lunch yet!", "Sugarplum|I'm just asking!",
            "Cherry|I know, I know. I'll see what we have.",
            "Cherry places the food on the table. Sugarplum starts eating.",
            "Cherry|Oh, I was thinking... maybe we could invite one of your old friends over sometime.",
            "Sugarplum|Really? Can we?", "Cherry|Of course. Is there someone you'd like to see?",
            "Sugarplum|Maybe... I haven't talked to them in a while.",
            "Cherry|That's okay. We can try calling the mom of one of them later.", "Sugarplum|Okay!",
            "Sugarplum goes back to eating. Cherry watches him for a moment before returning to her own meal."
        }},
        { (2, "Mensagem"), new[] {
            "After lunch, Cherry takes out her phone to contact the mother of one of Sugarplum's friends.",
            "Cherry|It's been so long. I hope they still keep in touch.",
            "Cherry searches through the contact list and finds the number.",
            "Cherry|Let's see... \"Hi! It's been a while. Would you and your son like to come over sometime?\"",
            "She sends the message.",
            "A different message pops up on the screen: This number is blocked. You will not receive messages or calls from this number.",
            "Cherry|And now that this is taken care of, let's see what's next on the house chore list."
        }},
        { (2, "LixoPensamento"), new[] { "Cherry|Huh. Must've been something in the bag." }},
        { (2, "PosLixo"), new[] {
            "Cherry finishes taking out the trash and returns to the apartment.",
            "She looks at the time.",
            "Cherry|Still got some time before dinner.",
            "Cherry gets the book she began reading yesterday and sits on the sofa."
        }},
        { (2, "Livro"), new[] {
            "Some time passes until Cherry closes the book, deciding to continue tomorrow.",
            "Cherry|I should start thinking about dinner."
        }},
        { (2, "PrepararJantar"), new[] { "Cherry|Let's get dinner ready." }},
        { (2, "JantarPronto"), new[] {
            "Sugarplum arrives a little before the dinner is complete, so he sits on the sofa waiting for his mother.",
            "Eventually the dinner gets done.",
            "Cherry|Dinner's ready!"
        }},
        { (2, "MesaJantar"), new[] {
            "Cherry and Sugarplum eat in silence for a few seconds.", "Cherry|So, how was your day?", "Sugarplum|Fine.", "Cherry|Did you get to play with anyone?",
            "Sugarplum|Not really.", "Cherry|Oh. How come?", "Sugarplum|They already have their own friends.",
            "Cherry|Well, it can take a little while to get to know people.", "Sugarplum|I tried talking to them, but they just went somewhere else.",
            "Cherry|Maybe they were busy. Give it some time, okay?", "Sugarplum|...",
            "Cherry|Oh! I almost forgot. I sent a message to your friend's mom about coming over this weekend.",
            "Sugarplum|Really?", "Cherry|Yep! We'll see what she says.", "Sugarplum looks a little more upbeat.",
            "After dinner, Cherry cleans the table while Sugarplum stays close, waiting for her.",
            "Sugarplum|Mommy, can we draw something?", "Cherry|Sure! Go get your paintbrushes."
        }},
        { (2, "Desenho"), new[] {
            "Cherry|What are we drawing today?", "Sugarplum|A forest!",
            "Cherry|A forest? All right. You draw the trees, and I'll draw the animals.",
            "Sugarplum|Can you draw a bear?", "Cherry|I can try!"
        }},
        { (2, "PosDesenho"), new[] {
            "Cherry puts the canvas on the table to let it dry.",
            "Cherry|It's nice to see him smile."
        }},
        { (2, "PreRemedio"), new[] { "Cherry|Time to get ready for bed." }},
        { (2, "Remedio"), new[] { "She takes her medication and puts it back on the shelf.", "Cherry|There. All done.", "Cherry turns off the light and goes to the bedroom." }},
        { (2, "HistoriaUrsos"), new[] {
            "Sugarplum|Can we read the bear story?", "Cherry|Of course. Where did we stop?", "Sugarplum|Berry was picking berries!", "Cherry|That's right. Let's see what happens next.", "Cherry opens the book and begins to read.",
            "Cherry|Berry didn't spend all her days wandering through the forest picking berries. She also helped take care of the sick animals. Everyone knew her as the forest healer.",
            "Cherry|She knew how to use all kinds of plants to help others. If someone had a cold, she knew how to prepare a nourishing meal. If someone had trouble sleeping, she knew how to make a tea that would help them rest. And if someone felt anxious, she knew how to make incense to calm their mind.",
            "Cherry|Her little cub admired her so much. He often told her that, when he grew up, he wanted to become a healer too.",
            "Cherry|And Berry always told him that if he kept eating his fruit, he'd grow even bigger than her and become a wonderful healer.",
            "Sugarplum|I'd eat all the plums!", "Cherry cracks a smile.", "Cherry|I bet you would.",
            "Cherry closes the book and puts it beside the bed.",
            "Cherry|Goodnight, my little Sugarplum. See you tomorrow.", "Sugarplum|Goodnight, Mommy.",
            "Day 2 — Complete"
        }},

        // DIA 3
        { (3, "Acordar"), new[] {
            "Cherry wakes up beside Sugarplum and checks the clock.",
            "Cherry|Another day...",
            "Cherry|I should get up before Sugarplum does. There's still a lot to do.",
            "#espelho",
            "She looks at herself in the mirror.",
            "Cherry|Have I always looked this exhausted?"
        }},
        { (3, "Cozinha"), new[] { "Cherry|Let's see what we have today. I don't feel like trying anything new." }},
        { (3, "AlmocoPronto"), new[] {
            "Cherry|Lunch is ready!",
            "Sugarplum comes over and sits at the table."
        }},
        { (3, "MesaAlmoco"), new[] {
            "Cherry|So, did you talk to anyone at school yesterday?", "Sugarplum|A little.", "Cherry|Oh? Who did you talk to?",
            "Sugarplum|Some kids from my class. They were playing together, but I didn't really know what to say.",
            "Cherry|That's okay. You don't have to become friends with everyone right away.",
            "Sugarplum|I tried joining them, but they didn't really notice me.",
            "Cherry|Maybe you could try talking to someone one-on-one. It might be easier.",
            "Sugarplum|Maybe... I still miss my old school, though.",
            "Cherry|I know, sweetheart. Starting over isn't easy.",
            "Sugarplum|Do you think I'll make friends eventually?",
            "Cherry|Of course you will. Just give it some time.",
            "They finish eating.",
            "Cherry|Finish getting ready, honey. It's almost time to go.", "Sugarplum|Okay, Mom.",
            "Cherry|Have a good day. Love you!", "Sugarplum|Love you too!",
            "Cherry watches him leave."
        }},
        { (3, "Brinquedos"), new[] {
            "Cherry notices toys scattered across the floor.",
            "Cherry|I should really put these away. It's getting hard to walk around here."
        }},
        { (3, "PosBrinquedos"), new[] {
            "After putting the last toy away, Cherry notices one lying on the floor again.",
            "Cherry|Didn't I just put that away?",
            "She puts it back in the box.",
            "Cherry|I must be more tired than I thought."
        }},
        { (3, "Livro"), new[] {
            "Cherry sits on the sofa and picks up Hamnet. She struggles to follow the page.",
            "Cherry|Where was I again?",
            "She rereads the same passage.",
            "Cherry|I'll try again later.",
            "She closes the book and gets up to prepare dinner."
        }},
        { (3, "PrepararJantar"), new[] { "Cherry|Time to make dinner. I hope Sugarplum comes home hungry." }},
        { (3, "JantarPronto"), new[] {
            "The sound of the knife continues briefly after Cherry stops cutting.",
            "Cherry|...That's weird.",
            "She checks the cutting board, then resumes cooking. She sets the table as Sugarplum returns."
        }},
        { (3, "MesaJantar"), new[] {
            "Cherry|Hey, honey. How was school?", "Sugarplum|Fine.", "Cherry|Did you try talking to anyone today?",
            "Sugarplum|Yeah. I tried talking to some kids, but they were already playing together.",
            "Cherry|Did you ask if you could join them?", "Sugarplum|No... I didn't want to bother them.",
            "Cherry|Sweetheart, asking to play isn't bothering anyone.", "Sugarplum|It's easy for you to say.",
            "Cherry|...You're right. I'm sorry. I know it's not that simple.",
            "Sugarplum|I just wish I could go back to my old school.",
            "Cherry|I know you miss it. I wish I could make this easier for you.",
            "Sugarplum|It's okay, Mom. I still have you.", "Cherry|Always, sweetheart.",
            "Cherry notices that Sugarplum has barely touched his food.",
            "Cherry|Are you full already?", "Sugarplum|Yeah. I'm not that hungry.", "Cherry|That's alright. I'll put the rest away.",
            "#cozinha",
            "For a moment, the food looks spoiled. Cherry blinks, and it looks normal again.",
            "Cherry|I really need some sleep."
        }},
        { (3, "Remedio"), new[] {
            "Cherry opens the medicine cabinet and checks her medication container.",
            "Cherry|Wait... that's all?", "The container is empty.",
            "Cherry|I thought I had a few left.", "Cherry|I'll have to get more tomorrow."
        }},
        { (3, "HistoriaUrsos"), new[] {
            "Cherry lies down beside Sugarplum and picks up the bedtime storybook.",
            "Sugarplum|Can you read the bear story tonight?", "Cherry|Of course. Where did we leave off?",
            "Sugarplum|The turtle!", "Cherry|Right. The turtle.",
            "Cherry|One day, Little Plum began to feel unwell. Berry tried everything she knew to help him feel better, but nothing seemed to work.",
            "Cherry|Worried, she decided to visit an old turtle who lived deep in the forest. He was known for his wisdom, and Berry hoped he might know what to do.",
            "Cherry|The old turtle listened carefully before speaking.",
            "Cherry|'You've done everything you can, Berry. For now, stay by his side and make sure he knows he's loved.'",
            "Cherry|Berry thanked him and returned home. She decided she would spend as much time with her little cub as she could.",
            "Sugarplum|Is Little Plum going to get better?", "Cherry|I'm sure Berry hopes so.",
            "Sugarplum|I hope he does.", "Cherry|Me too, sweetheart.",
            "Cherry closes the book.",
            "Cherry|That's enough for tonight. We can finish the story tomorrow.",
            "Sugarplum|Okay. Goodnight, Mommy.", "Cherry|Goodnight, my little Sugarplum. See you tomorrow.",
            "Cherry turns off the light. The ticking clock continues as the image blurs and fades to black.",
            "Day 3 — Complete"
        }},

        // DIA 4 (falas do Sugarplum sem rosto = vazias)
        { (4, "Acordar"), new[] {
            "Cherry|Morning, Cherry.",
            "Cherry|You need to get more pills. But before, let's make Sugarplum's meal, he comes first."
        }},
        { (4, "Cozinha"), new[] { "Cherry|I'm just gonna make the same as yesterday, can't think of any ideas for a new meal right now." }},
        { (4, "AlmocoPronto"), new[] {
            "The food smells like metal.",
            "Sugarplum appears in the scene, but he has no face.",
            "Cherry|Oh, you are already awake, just in time, the food is ready.",
            "Both sit at the table."
        }},
        { (4, "MesaAlmoco"), new[] {
            "Cherry|I know things didn't work out yesterday, but you should try again.",
            "Sugarplum|            ",
            "Cherry|How about you go to talk to different classmates today?",
            "Sugarplum|                        ",
            "Cherry|I promise to stop bringing this up after today's attempt, just try chatting with everyone first, ok?",
            "Sugarplum|            ",
            "Cherry|That's my boy, I'm proud of you.",
            "They keep eating.",
            "After the meal...",
            "Cherry|Good luck today, I believe in you. Love you.",
            "Sugarplum|      ",
            "Cherry|And he is going to school. Now is the time to get the medicine. I'll just save a trip downstairs and take the trash with me."
        }},
        { (4, "LixoPensamento"), new[] {
            "A liquid begins to ooze out of the trash. It looks like water.",
            "A sound echoes from the trash. It sounds like someone weeping.",
            "As she takes the trash out of the apartment, the weeping transforms into sobbing, and tears drip from the trash onto the floor.",
            "Something doesn't want to leave.",
            "Something inside the trash bawls its eyes out, until it gets out of sight as Cherry passes through the door."
        }},
        { (4, "PosLixo"), new[] {
            "Cherry|I feel so much better, this thing sure does miracles.",
            "Cherry|Well, what was I doing? Right, I took out the trash, so I have the afternoon free.",
            "Cherry|Let's do what I've been doing these past few days, read the book my mother wants me to read."
        }},
        { (4, "Livro"), new[] {
            "Cherry picks the book from the bookshelf and reads on the sofa for some time.",
            "Cherry checks the time.",
            "Cherry|I should make dinner, Sugarplum will soon be home.",
            "Cherry places the book back in the bookshelf.",
            "Cherry|I can probably finish it tomorrow, there is just a couple of pages left."
        }},
        { (4, "PrepararJantar"), new[] { "Cherry|Time to make dinner." }},
        { (4, "JantarPronto"), new[] { "Cherry wraps up the meal and sets the table. Sugarplum opens the door with a sad expression." }},
        { (4, "MesaJantar"), new[] {
            "Cherry|Things didn't go well?",
            "Sugarplum|No... doesn't matter who I try to talk to, not even a single one acknowledges me.",
            "Cherry|I'm so sorry Sugarplum, I really did think the suggestions would work.",
            "Sugarplum|It's ok mom, even if I don't make friends at school, I still have you to play with me at home.",
            "Cherry|Oh sweetie, I'm happy to hear this.",
            "Cherry|Is there something I can do to bring up the mood?",
            "Sugarplum|Can we draw together again?",
            "Cherry|Of course.",
            "They both eat dinner, chatting about what to draw."
        }},
        { (4, "Desenho"), new[] {
            "Cherry|So, are we drawing frogs today?",
            "Sugarplum|That's right.",
            "Cherry|Ok, you draw one on the right and I draw one on the left, deal?",
            "Sugarplum|Deal."
        }},
        { (4, "PosDesenho"), new[] {
            "Sugarplum|I think I did better than last time.",
            "Cherry|Yes, that's how it usually goes, maybe some day you'll get to my level.",
            "Sugarplum|Just you wait, I know I will.",
            "Cherry|Great, but now it's time to go to sleep. I'll tell the next part of the story, wait for me in bed.",
            "Sugarplum|OK.",
            "The little child goes running to the room to get ready."
        }},
        { (4, "PreRemedio"), new[] { "Cherry|I did it, I passed through today." }},
        { (4, "Remedio"), new[] {
            "Cherry|I should take another pill, don't wanna risk feeling under the weather again.",
            "Cherry takes her second pill of the day."
        }},
        { (4, "HistoriaUrsos"), new[] {
            "Cherry|All ready for the next part?",
            "Sugarplum|Yes, read it!",
            "Cherry opens the book right where she left it.",
            "Cherry|Following the advice of the wise turtle, Berry made sure to bring the fruits that Little Plum loves the most every day.",
            "Cherry|She tells him bedtime stories every night, and she always says to him, before going out in the woods, that he will become better soon enough.",
            "Cherry|After a few weeks following the same new routine, Berry came back to her home carrying various fruits.",
            "Cherry|When she came across the silhouette of Little Plum looking different compared to the other days...",
            "Cherry|He was lying down on her bed, but didn't greet her with a smile like always. She gets closer and puts the fruits in front of him, but her cub doesn't move.",
            "Cherry|She asks what is wrong, but her son doesn't respond. She places her hand on his forehead to check if the fever got worse, and it's at this moment that the mother realises...",
            "Cherry|Her son isn't breathing.",
            "Cherry|The mother calls her son's name, but there is no answer.",
            "Cherry|Her breathing begins to fail.",
            "Cherry|The mother shakes her son's body, but there is no answer.",
            "Cherry|Her throat begins to tighten.",
            "Cherry|The mother shouts for her son, but her voice doesn't come out.",
            "Cherry|Her lungs grasp for air in vain.",
            "Cherry|She can't feel her breath, as if there are hands gripping her neck, tightening every second.",
            "Cherry|The mother collapses to the floor, letting go of her child's cold corpse. The impact on the ground forces his eyes open, gazing at her, judging her.",
            "Cherry|'Why did this happen to me?' - The gaze questions.",
            "Cherry|'Why did you let this happen to me?' - The gaze inquires.",
            "Cherry|'Why didn't you do enough?' - The gaze accuses.",
            "Cherry|'It's because you are a despicable mother.' - The gaze declares.",
            "Cherry|'YOU FAILED YOUR SON.' - The gaze screams.",
            "Cherry|stop",
            "Cherry|'YOU ARE NOT WORTH TO BE CALLED A MOTHER.' - The voices in her head yell.",
            "Cherry|Stop",
            "Cherry|'YOU ARE GOING TO JOIN YOUR SON.' - She screeches to herself.",
            "Cherry|STOP",
            "Cherry|The grip in her throat intensifies, air no longer reaching her lungs, her voice no longer coming out of her mouth, her vision becomes blurry.",
            "Yet her son's lifeless eyes still gaze directly at her soul.",
            "She feels a small hand grabbing her arm, she slowly turns to look at the origin of that touch...",
            "Sugarplum|Mom, are you ok? You stopped moving all of a sudden.",
            "Her son looks at her with a worried expression on his face.",
            "Cherry snaps out of the trance.",
            "She looks at the book and reads: Berry can't help but open a big smile, seeing her cub well again and on top of that thinking of her right after getting better. She takes the gift and both of them eat their fruits together.",
            "Cherry|I guess I shouldn't have taken another pill, I'm seeing things again.",
            "Cherry|Ah, sorry Sugarplum, I didn't sleep well last night, I guess I'm more tired than I thought.",
            "Sugarplum|You should sleep then, we can finish the story tomorrow.",
            "Sugarplum|Goodnight Mommy.",
            "Cherry|Goodnight, my little Sugarplum. See you tomorrow.",
            "Day 4 — Complete"
        }},

        // DIA 5
        { (5, "Acordar"), new[] {
            "Cherry wakes up beside Sugarplum.",
            "Cherry|Morning, Cherry.",
            "Cherry|Now that's what I call a good night's sleep.",
            "#espelho",
            "She looks at herself in the mirror.",
            "Cherry|I feel great today.",
            "Cherry|Sugarplum's been so happy to have me around lately. It's nice knowing that, even with everything going on at school, he always has someone to come home to.",
            "#quarto",
            "Cherry|Come on, sleepyhead! Let's get this day started!",
            "Sugarplum|I'm getting up!"
        }},
        { (5, "Cozinha"), new[] { "Cherry|Let's make something nice today. It's the last day of the week, after all." }},
        { (5, "AlmocoPronto"), new[] {
            "The kitchen sounds linger after Cherry stops cooking.",
            "Cherry|Lunch is ready!"
        }},
        { (5, "MesaAlmoco"), new[] {
            "Cherry|Oh, I almost forgot.",
            "Cherry takes out the drawing she made a few days ago.",
            "Cherry|I made this for you. Remember all those frogs we were drawing?",
            "Sugarplum|You drew all of these?",
            "Cherry|Every single one. And look, I even gave you the biggest frog.",
            "Sugarplum|He looks so silly!",
            "Cherry|Excuse me? I worked very hard on that silly frog.",
            "Sugarplum|I'm keeping it forever!",
            "Cherry|You'd better. I spent a whole afternoon on it.",
            "Sugarplum|Thanks, Mom. I love it.",
            "Cherry|I'm glad, sweetheart.",
            "The food on Sugarplum's plate briefly looks dark and spoiled.",
            "Cherry|Make sure you eat enough before school. I don't want you coming home starving again.",
            "The food looks normal again."
        }},
        { (5, "Brinquedos"), new[] {
            "Cherry opens the box containing the art supplies.",
            "Cherry|We really should keep these things organized. It'd make everything easier to find.",
            "Sugarplum's supplies look almost unused. Cherry's are worn and covered in repeated drawings.",
            "Cherry|Huh. Guess I've been using these more than I thought.",
            "Cherry|I should buy some new ones when I get the chance."
        }},
        { (5, "PosBrinquedos"), new[] {
            "She closes the box. The drawing she gave Sugarplum lies beside the bed, unnoticed."
        }},
        { (5, "Livro"), new[] {
            "Cherry sits on the sofa and opens Hamnet to the final pages.",
            "Cherry|Almost done.",
            "She reaches the final page and closes the book.",
            "Cherry|...That's it?",
            "Cherry|I still don't understand why Mom wanted me to read this so badly.",
            "Cherry|I've never liked sad stories.",
            "Her phone rings. Her mother's name appears on the screen.",
            "Cherry|I'll call her back later.",
            "The ringing stops. Cherry puts the book back on the shelf."
        }},
        { (5, "PrepararJantar"), new[] { "Cherry|Time to make dinner. Sugarplum should be home soon." }},
        { (5, "JantarPronto"), new[] {
            "The knife continues scraping after Cherry stops cutting. A dark stain spreads across the counter.",
            "Cherry|I really should clean this kitchen more often.",
            "She wipes the counter and finishes cooking.",
            "Cherry|Right on time!"
        }},
        { (5, "MesaJantar"), new[] {
            "Cherry|So, how was school today?",
            "Sugarplum|Good! I was actually looking forward to coming home.",
            "Cherry|Oh? Why's that?",
            "Sugarplum|Because I wanted to spend time with you.",
            "Cherry|Aw, sweetheart. You know I love spending time with you, too.",
            "Sugarplum|I know! I just like being here.",
            "Cherry|Well, I'm happy to have you here.",
            "Sugarplum|Oh! We had to write something for literature class today.",
            "Cherry|You did? What was it about?",
            "Sugarplum|We could write about something important to us.",
            "Cherry|That sounds interesting. What did you write?",
            "Sugarplum|It's a surprise. But can you check it for mistakes after dinner?",
            "Cherry|Of course. Bring it over when we're done.",
            "Sugarplum|Okay!",
            "For a brief moment, the chair across from Cherry is empty. The opposite place setting is untouched.",
            "Cherry|Don't forget your paper, okay?",
            "Sugarplum|I won't!",
            "The table returns to its previous appearance."
        }},
        { (5, "Licao"), new[] {
            "Cherry sits on the sofa as Sugarplum hands her his literature assignment.",
            "Cherry|Let's see what you've got here.",
            "Sugarplum|Don't laugh, okay?",
            "Cherry|I won't laugh. Promise."
        }},
        { (5, "PosLicao"), new[] {
            "Cherry pauses at the final line.",
            "Cherry|That's... quite a poem, honey.",
            "Sugarplum|Is it good?",
            "Cherry|Of course. You used some really interesting words.",
            "Sugarplum|My teacher said we could write about something important to us.",
            "Cherry|Well, you certainly put a lot of thought into it.",
            "Sugarplum|Can you check if I made any mistakes?",
            "Cherry|Let's see.",
            "Cherry reads the poem again, but doesn't question its meaning.",
            "Cherry|Your spelling could use a little work, but that's what practice is for.",
            "Sugarplum|So it's okay?",
            "Cherry|It's more than okay. You should be proud of yourself.",
            "Sugarplum|Thanks, Mom!",
            "Cherry folds the paper and places it on the table.",
            "Cherry|Now, let's get ready for bed. I promised you the last part of the bear story."
        }},
        { (5, "PreRemedio"), new[] {
            "Cherry looks at herself in the mirror. Her reflection appears cheerful and rested.",
            "Cherry|What a lovely day.",
            "Cherry|I think I've finally gotten back into a good routine."
        }},
        { (5, "Remedio"), new[] { "She takes her usual dose of medication and puts the container away." }},
        { (5, "HistoriaUrsos"), new[] {
            "Cherry|Just one more night.",
            "Cherry|The mother bear arrives at her den, carrying a basket full of fruit for her cub.",
            "Cherry|One more day, one more lonely trip through the forest.",
            "Cherry|The mother bear gently places a plum beside her cub's bed.",
            "Cherry|One more day, one more fruit added to the pile.",
            "Cherry|The mother bear strokes the fur on her child's head.",
            "Cherry|One more day, one more gesture of affection left unanswered.",
            "Cherry|The mother bear talks with her son about their day.",
            "Cherry|One more night, and her son's lips remain still.",
            "Cherry|The mother bear tells the next part of her son's favorite story.",
            "Cherry|One more night, one more story falling on deaf ears.",
            "Cherry|He's asleep.",
            "The mother bear leans closer to the empty pillow and kisses it.",
            "Cherry|Goodnight, my little Sugarplum. See you tomorrow.",
            "The camera pulls away, revealing the empty bed and Sugarplum's untouched belongings.",
            "Cherry smooths the blanket on the empty side of the bed.",
            "Cherry is alone.",
            "She lies down and turns off the light. The clock keeps ticking.",
            "Tomorrow, Cherry will wake up and do it all again.",
            "Day 5 — Complete"
        }}
    };

    HashSet<(int, string)> falasVistas = new();
    string[] falasAtuais;
    int falaAtual = 0;

    Dictionary<string, Color> coresPersonagem = new()
    {
        { "Cherry", new Color("ffb6c1") },
        { "Sugarplum", new Color("87ceeb") }
    };
    Color corPadrao = Colors.White;

    private void TocarMusica(string caminho)
    {
        var nova = GD.Load<AudioStream>(caminho);
        if (musica.Stream == nova && musica.Playing) return;
        musica.Stream = nova;
        musica.Play();
    }

    public override void _Ready()
    {
        gameState = GetNode<GameState>("/root/GameState");
        gameState.DiaMudou += AoDiaMudar;
        gameState.TarefaConcluida += AoTarefaConcluir;
        musica = GetNode<AudioStreamPlayer2D>("MusicaPrincipal");
        somDialogo = GetNode<AudioStreamPlayer2D>("somDialogo");

        var menu = GetNode<Node2D>("ComodoMenu");
        var quarto = GetNode<Node2D>("ComodoQuarto");
        var banheiro = GetNode<Node2D>("ComodoBanheiro");
        var espelho = GetNode<Node2D>("ComodoEspelho");
        var sala = GetNode<Node2D>("SalaDeEstar");
        var cozinha = GetNode<Node2D>("ComodoCozinha");
        var mesa = GetNode<Node2D>("ComodoCozinhaMesa");
        var cozinhar = GetNode<Node2D>("Cozinhar");
        var salafilho = GetNode<Node2D>("SalaFilho");
        var livro = GetNode<Node2D>("ComodoLivro");
        var areaExterna = GetNode<Node2D>("AreaExterna");

        comodos = new[] { menu, quarto, banheiro, espelho, sala, cozinha, mesa, cozinhar, salafilho, livro, areaExterna };

        // Segurança: se sobrou um nó "SalaPinceis" na cena (versão antiga), esconde para não aparecer por cima do menu.
        GetNodeOrNull<Node2D>("SalaPinceis")?.Hide();
        painelTarefa = GetNode<CanvasLayer>("PainelTarefa");
        MostrarComodo(menu);

        GetNode<Button>("%IrCozinha").Pressed += () => { MostrarComodo(cozinha); TentarFalar("Cozinha"); };
        GetNode<Button>("%DobrarRoupa").Pressed += IniciarMinigameRoupa;
        GetNode<Button>("%LevarLixo").Pressed += IniciarMinigameLixo;
        GetNode<Button>("%TarefaEscolar").Pressed += IniciarMinigameLicao;

        GetNode<Button>("%BotaoComecar").Pressed += () => { MostrarComodo(espelho); TentarFalar("Espelho"); GetNode<Button>("%SairEspelho").Show(); };
        GetNode<Button>("%IrBanheiro").Pressed += () => MostrarComodo(banheiro);
        GetNode<Button>("%VoltarBanheiro").Pressed += IrParaOQuarto;
        GetNode<Button>("%IrAreaExterna").Pressed += () => MostrarComodo(areaExterna);

        botaoJogarLixo = new Button { Text = "Jogar lixo", Position = new Vector2(500, 300), Size = new Vector2(200, 60) };
        botaoJogarLixo.Pressed += JogarLixo;
        areaExterna.AddChild(botaoJogarLixo);
        var voltarExterna = new Button { Text = "Voltar", Position = new Vector2(50, 50) };
        voltarExterna.Pressed += () => MostrarComodo(banheiro);
        areaExterna.AddChild(voltarExterna);

        GetNode<Button>("%Espelho").Pressed += () =>
        {
            MostrarComodo(espelho);
            GetNode<Button>("%SairEspelho").Show();
            if (gameState.PossoFazerATarefa(GameState.TomarRemedio) && !remedioTomado)
                SeguirParaORemedio();
        };
        GetNode<Button>("%SairEspelho").Pressed += () => MostrarComodo(banheiro);

        GetNode<Button>("%IrSala").Pressed += () =>
        {
            MostrarComodo(sala);
            if (gameState.PossoFazerATarefa(GameState.TarefaEscolar) && gameState.TarefaEstaFeita(GameState.Janta) && !licaoLiberada)
                TentarFalar("Licao", () => { licaoLiberada = true; AtualizarBotoes(); });
        };
        GetNode<Button>("%VoltarDaSala").Pressed += IrParaOQuarto;

        GetNode<Button>("%Livro").Pressed += () =>
        {
            MostrarComodo(livro);
            TentarFalar("Livro", () =>
            {
                livroLido = true;
                gameState.ConcluirTarefa(GameState.LerLivro);
                AtualizarBotoes();
                if (gameState.Day >= 2) { MostrarComodo(cozinha); TentarFalar("PrepararJantar"); }
            });
        };
        GetNode<Button>("%SairLivro").Pressed += () => MostrarComodo(sala);
        GetNode<Button>("%FogaoCozinhar").Pressed += IniciarMinigameCozinha;
        GetNode<Button>("%VoltarDaCozinha").Pressed += () => MostrarComodo(sala);

        GetNode<Button>("%IrSalaDaCozinha").Pressed += () =>
        {
            if (gameState.Day != 1 || gameState.TarefaEstaFeita(GameState.Janta)) MostrarComodo(sala);
            else { MostrarComodo(salafilho); TentarFalar("SalaComFilho", () => TentarFalar("fundosala")); }
        };
        GetNode<Button>("%sair").Pressed += () => { MostrarComodo(mesa); TentarFalar("Mesa"); };

        GetNode<Button>("Dialogo/Caixa/Avancar").Pressed += () =>
        {
            if (falasAtuais == null) return;
            if (tweenTexto != null && tweenTexto.IsRunning())
            {
                tweenTexto.Kill(); somDialogo?.Stop();
                GetNode<Label>("Dialogo/Caixa/Texto").VisibleRatio = 1;
                return;
            }
            falaAtual++;
            MostrarProximaLinha();
        };

        GetNode<Button>("%TomarRemedio").Pressed += () =>
        {
            if (!gameState.PossoFazerATarefa(GameState.TomarRemedio)) return;
            GetNode<Button>("%TomarRemedio").Hide();
            TentarFalar("Remedio", AoTomarRemedio);
        };

        GetNode<Button>("%AjeitarPinceis").Pressed += IniciarArrumarPinceis;

        Button botaoEspelho = GetNode<Button>("%Espelho");
        Label textoEspelho = GetNode<Label>("%TextoEspelho");
        textoEspelho.Hide();
        botaoEspelho.MouseEntered += () => textoEspelho.Show();
        botaoEspelho.MouseExited += () => textoEspelho.Hide();

        Button nextDay = GetNode<Button>("%ProximoDia");
        Label textoNextDay = GetNode<Label>("%TextoNextDay");
        nextDay.Visible = gameState.DiaCompleto();
        textoNextDay.Hide();
        nextDay.MouseEntered += () => textoNextDay.Show();
        nextDay.MouseExited += () => textoNextDay.Hide();
        nextDay.Pressed += OnButtonPressed;
    }

    private void JogarLixo()
    {
        if (!gameState.PossoFazerATarefa(GameState.LevarLixo)) return;
        gameState.ConcluirTarefa(GameState.LevarLixo);
        TentarFalar("LixoPensamento", () => { MostrarComodo(GetNode<Node2D>("SalaDeEstar")); TentarFalar("PosLixo"); });
    }

    // Dias 2 e 4: desenho antes do remédio. Dia 5: poema (lição) antes do remédio.
    private void AposJantar()
    {
        if (gameState.Day == 2 || gameState.Day == 4) { IniciarDesenho(); return; }
        if (gameState.Day == 5) { LiberarLicao(); return; }
        if (gameState.Day >= 2) { SeguirParaORemedio(); return; }
        LiberarLicao();
    }

    // Desenhar com o filho (dias 2 e 4). Se criar um minigame de desenho, chame-o entre "Desenho" e "PosDesenho".
    private void IniciarDesenho()
    {
        MostrarComodo(GetNode<Node2D>("SalaDeEstar"));
        TentarFalar("Desenho", () => TentarFalar("PosDesenho", () =>
        {
            if (gameState.PossoFazerATarefa(GameState.Brincar))
                gameState.ConcluirTarefa(GameState.Brincar);
            SeguirParaORemedio();
        }));
    }

    // Vai ao espelho e toma o remédio (depois conta a história).
    private void SeguirParaORemedio()
    {
        MostrarComodo(GetNode<Node2D>("ComodoEspelho"));
        TentarFalar("PreRemedio", () => TentarFalar("Remedio", AoTomarRemedio));
    }

    private void LiberarLicao()
    {
        MostrarComodo(GetNode<Node2D>("SalaDeEstar"));
        TentarFalar("Licao", () => { licaoLiberada = true; AtualizarBotoes(); });
    }

    private void AoTomarRemedio()
    {
        if (remedioTomado) return;
        remedioTomado = true;
        gameState.ConcluirTarefa(GameState.TomarRemedio);
        ContarHistoria();
    }

    private void ContarHistoria()
    {
        if (historiaContada) return;
        GD.Print("Objetivo: contar a história.");
        MostrarComodo(GetNode<Node2D>("ComodoQuarto"));
        TentarFalar("HistoriaUrsos", AoFimDaHistoria);
    }

    private void IrParaOQuarto()
    {
        MostrarComodo(GetNode<Node2D>("ComodoQuarto"));
        if (remedioTomado && !historiaContada && gameState.PossoFazerATarefa(GameState.HistoriaFilho))
            ContarHistoria();
    }

    private void AoFimDaHistoria()
    {
        if (historiaContada) return;
        historiaContada = true;
        gameState.ConcluirTarefa(GameState.HistoriaFilho);
        GD.Print("História contada.");
        GD.Print("Cherry e Sugarplum vão dormir.");
        FecharDialogo();
        if (gameState.Day <= 4)
        {
            GD.Print("Dia " + gameState.Day + " concluído. Avançando.");
            gameState.TentarVirarODia();
        }
        else GD.Print("Fim do jogo."); // Dia 5: coloque aqui a tela final
    }

    private void MostrarComodo(Node2D comodo, bool fecharDialogo = true)
    {
        foreach (var c in comodos) c.Hide();
        comodo.Show();
        painelTarefa.Visible = comodo != comodos[0];
        if (fecharDialogo) GetNode<CanvasLayer>("Dialogo").Hide();
        AtualizarBotoes();
    }

    private void AtualizarBotoes()
    {
        GetNode<Button>("%ProximoDia").Visible = gameState.DiaCompleto();
        GetNode<Button>("%TarefaEscolar").Visible = licaoLiberada && gameState.PossoFazerATarefa(GameState.TarefaEscolar);
        GetNode<Button>("%FogaoCozinhar").Visible = gameState.PossoFazerATarefa(GameState.Almoco) || gameState.PossoFazerATarefa(GameState.Janta);
        GetNode<Button>("%DobrarRoupa").Visible = livroLido && gameState.PossoFazerATarefa(GameState.DobrarRoupa);
        GetNode<Button>("%Livro").Visible = gameState.PossoFazerATarefa(GameState.LerLivro);
        GetNode<Button>("%Cavalete").Visible = gameState.PossoFazerATarefa(GameState.Brincar);
        GetNode<Button>("%TomarRemedio").Visible = gameState.PossoFazerATarefa(GameState.TomarRemedio);
        GetNode<Button>("%AjeitarPinceis").Visible = gameState.PossoFazerATarefa(GameState.AjeitarBrinquedos);

        //Jogar lixo
        bool podeLevarLixo = gameState.PossoFazerATarefa(GameState.LevarLixo);

        GetNode<Button>("%IrAreaExterna").Visible = podeLevarLixo;
        GetNode<Button>("%LevarLixo").Visible = podeLevarLixo;

        if (botaoJogarLixo != null)
            botaoJogarLixo.Visible = podeLevarLixo;
    }

    private void IniciarMinigameCozinha()
    {
        string tarefaAtual;
        if (gameState.PossoFazerATarefa(GameState.Almoco)) tarefaAtual = GameState.Almoco;
        else if (gameState.PossoFazerATarefa(GameState.Janta)) tarefaAtual = GameState.Janta;
        else return;

        // Almoço e jantar usam o minigame de precisão. No jantar do Dia 3 usa a cópia com as imagens da carne.
        string caminhoCena = "res://Scenes/minigame_precisao_carne.tscn";
        if (tarefaAtual == GameState.Janta && gameState.Day == 3)
        {
            string caminhoCarne = AcharCena(
                "res://Scenes/minigame_precisao_carne.tscn",
                "res://Scenes/MinigamePrecisaoCarne.tscn");
            if (caminhoCarne != null) caminhoCena = caminhoCarne;
            else GD.PrintErr("Cena minigame_precisao_carne.tscn não encontrada em res://Scenes/. Usando o minigame de precisão normal.");
        }
        PackedScene cena = GD.Load<PackedScene>(caminhoCena);
        if (cena == null) { GD.PrintErr("Não foi possível carregar o minigame."); return; }
        Node minigame = cena.Instantiate();
        minigame.TreeExited += () =>
        {
            if (!IsInsideTree()) return;
            gameState.ConcluirTarefa(tarefaAtual);
            Show();
            if (tarefaAtual == GameState.Janta)
            {
                MostrarComodo(GetNode<Node2D>("ComodoCozinha"));
                TentarFalar("JantarPronto", () => { MostrarComodo(GetNode<Node2D>("ComodoCozinhaMesa")); TentarFalar("MesaJantar", AposJantar); });
            }
            else if (gameState.Day == 5)
            {
                MostrarComodo(GetNode<Node2D>("ComodoCozinha"));
                TentarFalar("AlmocoPronto", () =>
                {
                    MostrarComodo(GetNode<Node2D>("ComodoCozinhaMesa"));
                    TentarFalar("MesaAlmoco", IniciarArrumarPinceis);
                });
            }
            else if (gameState.Day == 4)
            {
                MostrarComodo(GetNode<Node2D>("ComodoCozinha"));
                TentarFalar("AlmocoPronto", () =>
                {
                    MostrarComodo(GetNode<Node2D>("ComodoCozinhaMesa"));
                    TentarFalar("MesaAlmoco", () => MostrarComodo(GetNode<Node2D>("SalaDeEstar")));
                });
            }
            else if (gameState.Day == 3)
            {
                MostrarComodo(GetNode<Node2D>("ComodoCozinha"));
                TentarFalar("AlmocoPronto", () =>
                {
                    MostrarComodo(GetNode<Node2D>("ComodoCozinhaMesa"));
                    TentarFalar("MesaAlmoco", IniciarArrumarPinceis);
                });
            }
            else if (gameState.Day == 2)
            {
                MostrarComodo(GetNode<Node2D>("ComodoCozinha"));
                TentarFalar("AlmocoPronto", () => { MostrarComodo(GetNode<Node2D>("ComodoCozinhaMesa")); TentarFalar("MesaAlmoco", () => { MostrarComodo(GetNode<Node2D>("SalaDeEstar")); TentarFalar("Mensagem"); }); });
            }
            else
            {
                // Dia 1: almoço pronto na cozinha, depois a conversa na mesa.
                MostrarComodo(GetNode<Node2D>("ComodoCozinha"));
                TentarFalar("AlmocoPronto", () => { MostrarComodo(GetNode<Node2D>("ComodoCozinhaMesa")); TentarFalar("Mesa"); });
            }
        };
        GetTree().Root.AddChild(minigame);
        Hide();
    }

    // Devolve o primeiro caminho que existir (evita erro por diferença de maiúsculas/minúsculas no nome).
    private static string AcharCena(params string[] caminhos)
    {
        foreach (var c in caminhos)
            if (ResourceLoader.Exists(c)) return c;
        return null;
    }

    private void IniciarMinigameRoupa()
    {
        if (!gameState.PossoFazerATarefa(GameState.DobrarRoupa)) return;
        PackedScene cena = GD.Load<PackedScene>("res://Scenes/MinigameRoupa/minigame_roupa.tscn");
        if (cena == null) { GD.PrintErr("Não foi possível carregar o minigame de roupa."); return; }
        Node minigame = cena.Instantiate();
        minigame.TreeExited += () =>
        {
            if (!IsInsideTree()) return;
            gameState.ConcluirTarefa(GameState.DobrarRoupa);
            Show(); MostrarComodo(GetNode<Node2D>("SalaDeEstar")); AtualizarBotoes(); TentarFalar("PosDobrarRoupa");
        };
        GetTree().Root.AddChild(minigame); Hide();
    }

    private void IniciarMinigameLixo()
    {
        if (!gameState.PossoFazerATarefa(GameState.LevarLixo)) return;
        PackedScene cena = GD.Load<PackedScene>("res://Scenes/MinigameLixo/minigame_lixo.tscn");
        if (cena == null) { GD.PrintErr("Não foi possível carregar o minigame do lixo."); return; }
        Node minigame = cena.Instantiate();
        minigame.TreeExited += () => { if (!IsInsideTree()) return; gameState.ConcluirTarefa(GameState.LevarLixo); Show(); AtualizarBotoes(); };
        GetTree().Root.AddChild(minigame); Hide();
    }

    private void IniciarMinigameLicao()
    {
        if (!gameState.PossoFazerATarefa(GameState.TarefaEscolar)) return;
        PackedScene cena = GD.Load<PackedScene>("res://Scenes/MinigameLicao/minigame_licao.tscn");
        if (cena == null) { GD.PrintErr("Não foi possível carregar o minigame de lição."); return; }
        Node minigame = cena.Instantiate();
        minigame.TreeExited += () =>
        {
            if (!IsInsideTree()) return;
            gameState.ConcluirTarefa(GameState.TarefaEscolar); Show(); AtualizarBotoes();
            // Dia 5: depois do poema vai direto para o remédio.
            TentarFalar("PosLicao", () => { if (gameState.Day == 5) SeguirParaORemedio(); });
        };
        GetTree().Root.AddChild(minigame); Hide();
    }

    private void IniciarArrumarPinceis()
    {
        if (!gameState.PossoFazerATarefa(GameState.AjeitarBrinquedos))
            return;

        // Dias 3 e 5: arrumar o quarto, com falas (dia 3 = brinquedos, dia 5 = caixa de materiais de arte).
        if (gameState.Day == 3 || gameState.Day == 5)
        {
            MostrarComodo(GetNode<Node2D>("ComodoQuarto"));
            TentarFalar("Brinquedos", () =>
            {
                gameState.ConcluirTarefa(GameState.AjeitarBrinquedos);
                TentarFalar("PosBrinquedos", () => { MostrarComodo(GetNode<Node2D>("SalaDeEstar")); AtualizarBotoes(); });
            });
            return;
        }

        gameState.ConcluirTarefa(GameState.AjeitarBrinquedos);
        AtualizarBotoes();
    }

    private void AoFimPinceis()
    {
        gameState.ConcluirTarefa(GameState.AjeitarBrinquedos);
        Show();
        MostrarComodo(GetNode<Node2D>("ComodoQuarto"));
        TentarFalar("PosBrinquedos", () => { MostrarComodo(GetNode<Node2D>("SalaDeEstar")); AtualizarBotoes(); });
    }

    private void TentarFalar(string local, Action aoTerminar = null)
    {
        var chave = (gameState.Day, local);
        if (!falas.TryGetValue(chave, out var linhas) || falasVistas.Contains(chave)) { aoTerminar?.Invoke(); return; }
        falasVistas.Add(chave); falasAtuais = linhas; falaAtual = 0; aoTerminarFala = aoTerminar; MostrarProximaLinha();
    }

    private void MostrarProximaLinha()
    {
        while (falasAtuais != null && falaAtual < falasAtuais.Length && falasAtuais[falaAtual].StartsWith("#"))
        { ExecutarComando(falasAtuais[falaAtual]); falaAtual++; }
        if (falasAtuais != null && falaAtual < falasAtuais.Length) MostrarFala(falasAtuais[falaAtual]);
        else FimDoDialogo();
    }

    private void ExecutarComando(string comando)
    {
        switch (comando)
        {
            case "#sala": MostrarComodo(GetNode<Node2D>("SalaDeEstar"), false); break;
            case "#quarto": MostrarComodo(GetNode<Node2D>("ComodoQuarto"), false); break;
            case "#espelho":
                MostrarComodo(GetNode<Node2D>("ComodoEspelho"), false);
                GetNode<Button>("%SairEspelho").Hide(); // evita quebrar o diálogo ao clicar em sair
                break;
            case "#cozinha": MostrarComodo(GetNode<Node2D>("ComodoCozinha"), false); break;
        }
    }

    private void FimDoDialogo()
    {
        GetNode<CanvasLayer>("Dialogo").Hide();
        var acao = aoTerminarFala; aoTerminarFala = null; acao?.Invoke();
    }

    private void FecharDialogo()
    {
        tweenTexto?.Kill(); somDialogo?.Stop(); GetNode<CanvasLayer>("Dialogo").Hide();
    }

    private void MostrarFala(string fala)
    {
        Label texto = GetNode<Label>("Dialogo/Caixa/Texto");
        string personagem = "";
        if (fala.Contains('|')) { var partes = fala.Split('|', 2); personagem = partes[0]; fala = partes[1]; }
        Color cor = coresPersonagem.TryGetValue(personagem, out var c) ? c : corPadrao;
        texto.AddThemeColorOverride("font_color", cor);
        texto.Text = fala; texto.VisibleRatio = 0; GetNode<CanvasLayer>("Dialogo").Show();
        tweenTexto?.Kill(); tweenTexto = CreateTween(); tweenTexto.TweenProperty(texto, "visible_ratio", 1.0, fala.Length * 0.05);
        tweenTexto.Finished += () => somDialogo?.Stop(); somDialogo?.Play();
    }

    public override void _ExitTree()
    {
        if (gameState != null) gameState.DiaMudou -= AoDiaMudar;
        if (gameState != null) gameState.TarefaConcluida -= AoTarefaConcluir;
    }

    private void OnButtonPressed() { gameState.TentarVirarODia(); }

    private void AoDiaMudar(int dia)
    {
        GD.Print("Dia mudou para: " + dia);
        livroLido = false; licaoLiberada = false; remedioTomado = false; historiaContada = false;
        MostrarComodo(GetNode<Node2D>("ComodoQuarto"));
        if (falas.ContainsKey((dia, "Acordar")))
        {
            GetTree().CreateTimer(2.5).Timeout += () =>
            {
                if (!IsInsideTree()) return;
                if (dia >= 3)
                {
                    // Dia 3, 4 e 5: acorda no quarto e termina na cozinha.
                    MostrarComodo(GetNode<Node2D>("ComodoQuarto"));
                    TentarFalar("Acordar", () => { MostrarComodo(GetNode<Node2D>("ComodoCozinha")); TentarFalar("Cozinha"); });
                    return;
                }
                MostrarComodo(GetNode<Node2D>("ComodoEspelho")); GetNode<Button>("%SairEspelho").Show(); TentarFalar("Acordar");
            };
        }
    }

    private void AoTarefaConcluir(string nomeTarefa) { AtualizarBotoes(); }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton { Pressed: true }) GD.Print("MOUSE CLICADO!");
        if (OS.IsDebugBuild() && @event is InputEventKey { Pressed: true, Echo: false } tecla)
        {
            if (tecla.Keycode == Key.F1) PularDiaDebug();
            else if (tecla.Keycode == Key.F2) gameState.DebugConcluirTarefasDoDia();
        }
    }

    private void PularDiaDebug()
    {
        tweenTexto?.Kill(); somDialogo?.Stop(); falasAtuais = null; aoTerminarFala = null;
        gameState.DebugPularDia();
    }
}