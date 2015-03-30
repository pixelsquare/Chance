using System.Collections.Generic;
using Item;
using Item.Database;
using MiniGame;
using UnityEngine;

namespace NPC {
    public enum NPCNameID {
        None,
        Nina,
        Beta,
        Noelle,
        Andy,
        Jenevieve,
        Franz,
        Bart,
        Maxine
    };

	public enum ButtonType {
		Correct, Wrong
	};

	public enum DialogueType {
		Continous, Random, Selection
	};

	public enum ReplyType {
		Accept, Decline
	};

	public enum RequirementType {
		Any, All
	};
	namespace Database {
		public struct NPCDatabase {
			private static NPCData[] npcDataInfoList = new NPCData[9];
			public static NPCData[] NPCDataInfoList {
				get { return npcDataInfoList; }
			}

			public static List<NPCData> sortingDataList = new List<NPCData>();

			public static void Initialize() {
				# region Test Nina
				npcDataInfoList[0] = new NPCData(
					"Nina",
					TextureResource.AvatarDatabase.NinaAvatar,
					NPCNameID.Nina,
					"A happy-go-lucky 18-year old yet short-tempered girl, who can’t be honest with her feelings.",
					new Statistics(34, 76, 23, 87),
					new ItemsNeeded[1] {
						new ItemsNeeded(ItemNameID.USB)
					},
					new NPCDialogue[3] {

					# region Script Dialogue 1
				        new NPCDialogue(
				            new DialogueData[4] {
				                new DialogueData("Have you chosen a teammate yet brother?"),
				                new DialogueData("I mean look at them. I'm not ready for this."),
				                new DialogueData("This makes me nervous."),
				                new DialogueData("What do you think, brother?", new DialogueButton[2] { 
				                    new DialogueButton("Stop that or I'll get nervous, too", 1, ButtonType.Wrong, new Statistics(1, 1, 1, 1, 0, 7)), 
				                    new DialogueButton("Don't be nervous. I'm here!", 2, ButtonType.Correct, new Statistics(2, 0)) 
				                })
				            }
				        ),
				    # endregion Script Dialogue 1

					# region Script Dialogue 2
				        new NPCDialogue(
				            new DialogueData[1] {
				                new DialogueData("Are you sure you're okay?", new DialogueButton[2] {
				                    new DialogueButton("It's none of your business. It's only a bump on the head", ButtonType.Correct, new DialogueData[2] {
				                        new DialogueData("Fine!"),
				                        new DialogueData("Hmph. Continue being stubborn and bleed yourself to death!")
				                    }),
				                    new DialogueButton("I'm fine! This is nothing. Nothing to worry about, see?", 2, ButtonType.Wrong, new DialogueData[3] {
				                        new DialogueData("What do you take me for?"),
				                        new DialogueData("You're bleeding, stupid!"),
				                        new DialogueData("What's nothing to worry about when it's clearly not okay.")
				                    })
				                })
				            }
				        ),
				    # endregion Script Dialogue 2

				    # region Script Dialogue 3
				        new NPCDialogue(
				            new DialogueData[2] {
				                new DialogueData("Why did you choose this course?"),
				                new DialogueData("I, for one, want this to create more life into games through my art.", new DialogueButton[2] {
				                    new DialogueButton("Because I love playing games?", ButtonType.Correct, new DialogueData[5] {
				                        new DialogueData("Go quit."),
				                        new DialogueData("You're not taking this seriously. Please don't force yourself."),
				                        new DialogueData("Go quit, if that's the only thing that drives you."),
				                        new DialogueData("If you can't see anything at the other end of the tunnel while riding in this course, you clearly don't love this course."),
				                        new DialogueData("Because you don't know where to go to.")
				                    }),
				                    new DialogueButton("I am in this course because, I want to create the games that I always dreamed of and share my dream games to other people.",											ButtonType.Wrong,
				                        new DialogueData[2] {
				                            new DialogueData("You're a little dreamer, but, I like it that you have a goal in mind."),
				                            new DialogueData("Let's have fun in this course!")
				                    })
				                })
				            }
				        )
				    # endregion Script Dialogue 3

					},
					new SympathyText(
						new string[1] { "Sorry"	}
					),
					new AcceptedText[1] {
						new AcceptedText(
						new DialogueData[1] { 
							new DialogueData("Thank you for giving me ") 
						})
					},
					new DeclinedText[1] {
						new DeclinedText(
						new DialogueData[1] { 
							new DialogueData("Sorry I don't need this") 
						})
					});
				# endregion Test Nina

				# region Test Beta
				npcDataInfoList[1] = new NPCData(
					"Beta",
					TextureResource.AvatarDatabase.BetaAvatar,
					NPCNameID.Beta,
					"A proud 17-year old who can’t help but brag about his luxurious lifestyle.",
					new Statistics(23, 86, 51, 35),
					new ItemsNeeded[1] {
						new ItemsNeeded(ItemNameID.Donut)
					},
					new NPCDialogue[3] {

					# region Script Dialogue 1
						new NPCDialogue(
							new DialogueData[5] {
								new DialogueData(TextureResource.AvatarDatabase.NinaAvatar,"Do you really think you can outsmart a robot?"),
								new DialogueData("I don't think you can."),
								new DialogueData("According to my calculations, no matter how large your brain is only a certain amount of percentage is used."),
								new DialogueData("Unlike robots, who are able to maximize themselves to their full potential."),
								new DialogueData("Can you do that?", new DialogueButton[2] {
									new DialogueButton("I'm not so sure anymore.", ButtonType.Wrong, new DialogueData[] { 
										new DialogueData("K, bye!")
									}),
									new DialogueButton("I can do it! Just watch me.", 2, ButtonType.Correct, new DialogueData[2] { 
										new DialogueData("Alright, I will give you a set of questions Each number on a different scale level."),
										new DialogueData("Are you sure you want to do this?", new DialogueButton[2] {
											new DialogueButton("Suddenly, I felt like doubting myself.", ButtonType.Wrong, new DialogueData[1] {
												new DialogueData("That's very disappointing. I was hoping you could prove me wrong.")
											}),
											new DialogueButton("Yes.", 1, ButtonType.Correct, new DialogueData[2] {
												new DialogueData("Very well then. Here are the questions."),
												new DialogueData("Good luck.")

											})
										})
									})
								})
							}
						),
					# endregion Script Dialogue 1

					# region Script Dialogue 2
						new NPCDialogue(
							new DialogueData[2] {
								new DialogueData("You have exceeded my expectations, human."),
								new DialogueData("How is it possible for you to answer those questions?", new DialogueButton[2] {
									new DialogueButton("I am not sure myself. I was born this way.", 2, ButtonType.Correct, new DialogueData[3] {
										new DialogueData("I am trying to compute the possibilities of humans born like you."),
										new DialogueData("But all I see is an error."),
										new DialogueData("It couldn't be possible.")
									}),
									new DialogueButton("Shouldn't you know the answer to your own question? You ARE a computer, aren't you?", ButtonType.Wrong, new DialogueData[1] {
										new DialogueData("If I was able to compute and deduce the answer, I wouldn't have asked, would I?")
									})
								})
							}
						),
					# endregion Script Dialogue 2

					# region Script Dialogue 3
						new NPCDialogue(
							new DialogueData[1] {
								new DialogueData("Do you consider me as a friend?", new DialogueButton[2] {
									new DialogueButton("Do you?", ButtonType.Correct, new DialogueData[2] {
										new DialogueData("I do."),
										new DialogueData("You are my first friend.")
									}),
									new DialogueButton("I am not sure if I can be friends with robots such as yourself.", ButtonType.Wrong, new DialogueData[2] {
										new DialogueData("Do you have something against robots?"),
										new DialogueData("I am very much harmless.")
									})
								})
							}
						)
					# endregion Script Dialogue 3

					},
					new SympathyText(
						new string[1] { "Sorry" }
					),
					new AcceptedText[1] {
						new AcceptedText(
						new DialogueData[1] { 
							new DialogueData("Thank you for giving me ") 
						})
					},
					new DeclinedText[1] {
						new DeclinedText(
						new DialogueData[1] { 
							new DialogueData("Sorry I don't need this") 
						})
					});

				# endregion Test Beta

                # region Noelle
                npcDataInfoList[2] = new NPCData(
                    "Noelle Ingram",
                    TextureResource.AvatarDatabase.PlaceholderAvatar,
                    NPCNameID.Noelle,
                    "An indecisive designer, who had just established her future goals. She is very knowledgeable about a lot of general things but, she doesn’t voice them out often in real life especially to people she just met unless required to.",
                    new Statistics(83, 93, 44, 56),
                    new ItemsNeeded[2] {
						new ItemsNeeded(ItemNameID.FanfictionNotebook), new ItemsNeeded(ItemNameID.StrawberryMilkshake)
					},
                    new NPCDialogue[10] {

					# region Dialogue 1
						new NPCDialogue(
							new DialogueData[1] {
								new DialogueData("Uh, hi.", new DialogueButton[2] {
									new DialogueButton("Hi there! Classmates last Tuesday right?", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
										new DialogueData("Yeah. And yesterday, too, but…"),
                                        new DialogueData("As far as I could remember from the surnames being called for attendance..."),
                                        new DialogueData("You weren’t there right?"),
										new DialogueData("Ah I didn’t mean to snoop around or anything, I really just remembered.")
									}),
									new DialogueButton("… Hi.", 6, ButtonType.Wrong, ToughnessLevel.Level1, new DialogueData[1] {
										new DialogueData("R-right /awkward silence/")
									})
								})
							}
						),
					# endregion Dialogue 1

					# region Dialogue 2
						new NPCDialogue(
							new DialogueData[4] {
                                new DialogueData("So, um… wha- ack! /cough cough/"),
                                new DialogueData("Excuse me. Sorry, that was a bit embarrassing. I can’t believe I got choked by air."),
                                new DialogueData("The nerves got to me. It’s just that…this is the first time I actually spoke to you one on one."),
								new DialogueData("Anyways, what I wanted to say before, what brings you here in school today? Class?", new DialogueButton[2] {
									new DialogueButton("Pfft. Hahaha! I can’t believe you got choked by air! Ahahahaha! " +
														"Ehem! Sorry about that. I wasn’t able to keep it to myself…", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3] {
										new DialogueData("Um, no. It’s okay. I’m kind of used to those kinds of reaction."),
										new DialogueData("Aaaaand I’m weird like that. I guess! Haha"),
										new DialogueData("But, hey, the tension’s gone at least, right?")
									}),
									new DialogueButton("Uh....I have class... no, I don't, I mean. I got to go.", 7, ButtonType.Wrong, ToughnessLevel.Level1, new DialogueData[4] {
										new DialogueData("Am I too weird?"),
										new DialogueData("I only did it by accident."),
                                        new DialogueData("Well, if you find me too weird and you'd rather stay away from me, then fine!"),
                                        new DialogueData("Go away..!")
									})
								})
							}
						),
					# endregion Dialogue 2

					# region Dialogue 3
						new NPCDialogue(
							new DialogueData[3] {
								new DialogueData("You know…you haven’t really answered my question before. But, nevermind that!"),
								new DialogueData("Let’s change topic… if that is you don’t mind."),
								new DialogueData("The question before was boring so might as well be random~", new DialogueButton[2] {
									new DialogueButton("I don’t mind at all! Let’s see topic…Ah! " +
														"How about interests? Any interests, besides games?", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5] {
										new DialogueData("I have a LOT actually. Where to start…"),
										new DialogueData("I like Anime, Manga and Kpop…uh…"),
                                        new DialogueData("I love food in general AND strawberry milkshakes or banana shakes"),
                                        new DialogueData("I love and do writing and reading fanfictions."),
										new DialogueData("I think; I should stop it until there. It’s really a long list I tell you!")
									}),
									new DialogueButton("Err, random? Uh, do you have any random topic in mind?", 8, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[4] {
										new DialogueData("Um, I was kinda hoping you’d be the one choosing the topic?"),
                                        new DialogueData("Y’know, you don’t have to force yourself talking with me."),
                                        new DialogueData("I-I think! I think I should go. Um, class…"),
										new DialogueData("I have class! Yeah, I have class… so yeah.")
									})
								})

						}),
					# endregion Dialogue 3

					# region Dialogue 4
						new NPCDialogue(new DialogueData[3] {
                            new DialogueData("So… what’s your role?"),
							new DialogueData("It’s either you’re an artist, programmer, designer or producer right?"),
                            new DialogueData("Take your pick~", new DialogueButton[3] {
								new DialogueButton("I can do art~", 5, ButtonType.Correct, new DialogueData[4] {
									new DialogueData("Oooh~ An artist!"),
									new DialogueData("Let me see your work~ :3"),
                                    new DialogueData("Pleeeaaaaassseeee~"),
                                    new DialogueData("Please Oh please oh please oh please oh please oh please!!!!")
								}),
                                new DialogueButton("I can program decently, I suppose.", 5, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
									new DialogueData("Do you have sample games I can try~?"),
									new DialogueData("What languages are you good at~?"),
                                    new DialogueData("I'll add you to my imaginary list of programmers in our batch~!"),
                                    new DialogueData("Hehe~ I do hope you participate in the incoming event")
								}),
								new DialogueButton("Put me in any spot! I can take care of everything!", 9, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[3] {
									new DialogueData("You're one cocky person, aren't you?"),
									new DialogueData("If you're that cocky, can I see any of your works?"),
									new DialogueData("I'll acknowledge your bragging rights when you have something to show me.")
								})
							})
						}),
					# endregion Dialogue 4

					# region Dialogue 5
						new NPCDialogue(new DialogueData[1] {
							new DialogueData("It’s that time again.", new DialogueButton[3] {
								new DialogueButton("Yeah, it's game jam again!", 10, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
									new DialogueData("That’s right. Cool, you’re participating, too!"),
                                    new DialogueData("Best of luck to the both of us!"),
                                    new DialogueData("Let’s conquer the three days and two nights of no sleep!"),
                                    new DialogueData("Not that I actually slept in the previous game jams since I did sleep unlike my teammate. Hahaha!")
								}),
                                new DialogueButton("Game Jam, right...?", 10, ButtonType.Correct, new Statistics(10, 0), new DialogueData[6] {
									new DialogueData("What's wrong? Not exciting enough?"),
                                    new DialogueData("Or maybe... discouraged?"),
                                    new DialogueData("Sigh, that's nonsense! Just give it your all!"),
                                    new DialogueData("We can do this! I mean it's not everyday you can do something like that right?"),
                                    new DialogueData("It's a once a year thing. While we have the time because we're students..."),
                                    new DialogueData("We should take advantage of it and enjoy, okay?")
								}),
								new DialogueButton("Time for what?", 10, ButtonType.Wrong, ToughnessLevel.Level3, new DialogueData[4] {
									new DialogueData("Oh, you know. Game jam?"),	
                                    new DialogueData("Erm, how do you not know that it’s game jam season?"),	
                                    new DialogueData("You didn't participate the previous years?"),
	                                new DialogueData("HOW COULD YOU NOT PARTICIPATE? Ehem...it WAS a REQUIREMENT in our year.")
								})
							})
						}),
					# endregion Dialogue 5

					# region Dialogue 6
						new NPCDialogue(new DialogueData[2] {
                            new DialogueData("Sigh, it’s so stressful."),
							new DialogueData("Ah-! You were here? Sorry, I guess I was too deep in thought.", new DialogueButton[3]{
								new DialogueButton("You should be careful next time. You might bump into something.", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
									new DialogueData("Oh, thanks."),
									new DialogueData("I’m pretty much a klutz so, it’s pretty normal if you see me bump into something or someone."),
									new DialogueData("Most of the time though, the reason why I bump into something because, I can only keep and focus my attention on something one at a time. Hahaha!"),
                                    new DialogueData("But…I’m doing my best multitasking…erm, I suppose…")
								}),
								new DialogueButton("What’s stressful?", 7, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[2] {
									new DialogueData("Ugh, please."),
									new DialogueData("Mind your own business.")	
								}),
                                new DialogueButton("You know. It would slightly help if you vent out those stress.", 7, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[1] {
									new DialogueData("I know that already.")
								})
							})
						}),
					# endregion Dialogue 6

					# region Dialogue 7
						new NPCDialogue(new DialogueData[3] {
							new DialogueData("Just great…JUST GREAT. I have to go and do this and this…"),
                            new DialogueData("Tsk."),
                            new DialogueData("Would you stop following me?", new DialogueButton[3] {
								new DialogueButton("What the hell’s your problem?", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5] {
									new DialogueData("Oh, um… sorry."),
	                                new DialogueData("I was just overwhelmed by the added things I have to do when I’m not even finished with others."),
                                    new DialogueData("And I..I’m hungry. I didn’t get to eat any for the day yet hehe.."),
                                    new DialogueData("I get cranky sometimes when I’m hungry..but that’s not excuse, so yeah"),
                                    new DialogueData("Sorry. Still...I apologize for that episode.")
								}),
								new DialogueButton("I'm not even following you...", 8, ButtonType.Wrong, ToughnessLevel.Level3, new DialogueData[3] {
									new DialogueData("Nevermind."),
	                                new DialogueData("Just go away, okay?"),
                                    new DialogueData("Clearly, I'm not in the mood.")
								}),
                                new DialogueButton("Why are you so irritated? Do you have your monthly period or something?", 8, ButtonType.Wrong, ToughnessLevel.Level3, new DialogueData[2] {
									new DialogueData("That's just!"),
	                                new DialogueData("Ugh! You're asking for it!")
								})
							})
						}),
					# endregion Dialogue 7

					# region Dialogue 8
						new NPCDialogue(new DialogueData[4] {
							new DialogueData("Yo! I've been seeing you more and more in the hallway lately!"),
                            new DialogueData("Not that it's bad or anything."),
                            new DialogueData("Um, let's see here. I actually wanted to ask you something"),
                            new DialogueData("Have you seen some kind of notebook around? A-Ah, not that there's something special in it or anything at all!", new DialogueButton[3] {
								new DialogueButton("I'm not sure if I've seen one. If ever I come across it, I'll give it to you.", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3]{
                                    new DialogueData("Oh~ Thank you thank you thank you thank you!"),
                                    new DialogueData("DON'T read it okay?"),
                                    new DialogueData("It's NOT interesting so just give it to me IMMEDIATELY.")
                                }),
								new DialogueButton("You're obvious, you know?" +
                                                    "Now, I'm quite interested on learning what's inside once I find it first.", 9, ButtonType.Wrong, ToughnessLevel.Level4, new DialogueData[3]{
                                    new DialogueData("Don't you dare!"),
                                    new DialogueData("Seriously, don't you DARE!"),
                                    new DialogueData("I WILL find out if you read it or even dared open it")
                                }),
                                new DialogueButton("I do wonder. Did I see one or not? Since you seem nervous about it," +
                                                    "I might as well check it out myself.", 9, ButtonType.Wrong, ToughnessLevel.Level4, new DialogueData[3]{
                                    new DialogueData("Nonononononononono!"),
                                    new DialogueData("Please I beg you! If it's with you, please don't!"),
                                    new DialogueData("T^T NOOOO~")
                                })
							})
						}),
					# endregion Dialogue 8

					# region Dialogue 9
						new NPCDialogue(new DialogueData[1] {
							new DialogueData("It's a good thing I have a laptop!", new DialogueButton[3] {
								new DialogueButton("Why is that?", 5, ButtonType.Correct, new Statistics(10, 0), new DialogueData[7]{
                                    new DialogueData("Well, if you’re planning to stay overnight in the school during the game jam"),
                                    new DialogueData("...you won’t have to worry and bring your desktop from home just to set it up in the venue."),
                                    new DialogueData("That would be a hassle, don’t you think? I mean…"),
                                    new DialogueData("Number 1, they’re heavy."),
                                    new DialogueData("Number 2, a lot of wires to setup and so little space in the venue."),
                                    new DialogueData("And Number 3, you’ll bring a lot of the parts like the CPU then the keyboard, then the monitor, etc."),
                                    new DialogueData("Not unless you have a car...")
                                }),
								new DialogueButton("Desktops are better than laptops!", 10, ButtonType.Wrong, ToughnessLevel.Level5, new DialogueData[3]{
                                    new DialogueData("Well, laptops are more convenient with this type of event!"),
                                    new DialogueData("Not unless you decide to go home and just come back the next day..."),
                                    new DialogueData("But still, a hassle!")
                                }),
                                new DialogueButton("...And then on the day of the event, it crashes.", 10, ButtonType.Wrong, ToughnessLevel.Level5, new DialogueData[2]{
                                    new DialogueData("Don't you dare jinx that."),
                                    new DialogueData("If that happens, I'll blame you.")
                                })
							})
						}),
					# endregion Dialogue 9

					# region Dialogue 10

                        // level6 mini game pls
						new NPCDialogue(new DialogueData[1] {
							new DialogueData("Hmm~ So excited", new DialogueButton[3] {
								new DialogueButton("Haha, you're that excited huh?", ButtonType.Correct, new Statistics(10, 0), new DialogueData[6]{
                                    new DialogueData("Weeeeell~ I have some ideas stacked up and ready for game jam."),
                                    new DialogueData("Though, I’ll worry about the theme on the day itself…Still!"),
                                    new DialogueData("I’m ready! I have my Unity, RPGMaker, Construct2, Renpy.."),
                                    new DialogueData("Erm I don’t have Game Maker though, but, I have Photoshop!"),
                                    new DialogueData("Then, there’s Maya…uh… how about you?"),
                                    new DialogueData("Are you prepared?")
                                }),
                                new DialogueButton("I look forward to the game jam, too. Just not as hyped up as you. Haha.", ButtonType.Correct, new Statistics(10, 0), new DialogueData[6]{
                                    new DialogueData("Oh, too hyper for you?"),
                                    new DialogueData("Sorry about that. I couldn't help it"),
                                    new DialogueData("I made preparations for it though!"),
                                    new DialogueData("Have you packed yet? Or... did you decide not to overnight?"),
                                    new DialogueData("Speaking of overnight, I could actually not stay overnight since my dorm is just a walk away."),
                                    new DialogueData("But it's more fun doing the jam with all your old and new friends.")
                                }),
								new DialogueButton("I'm not excited at all. I'm more nervous.", ButtonType.Wrong, ToughnessLevel.Level5, new DialogueData[2]{
                                    new DialogueData("Don't be! I'm letting all this excitement out to remove my anxiety."),
                                    new DialogueData("If you keep on being nervous over there, you'll affect my excitement!")
                                })
							})
						}),
					# endregion Dialogue 10

					},
					new SympathyText(
						new string[1] { "Sorry" }
					),
					new AcceptedText[1] {
						new AcceptedText(
						new DialogueData[1] { 
							new DialogueData("Thank you for giving me ") 
						})
					},
					new DeclinedText[1] {
						new DeclinedText(
						new DialogueData[1] { 
							new DialogueData("Sorry I don't need this") 
						})
					});
                # endregion Noelle

                # region Andy
                npcDataInfoList[3] = new NPCData(
                    "Andy Gibson",
                    TextureResource.AvatarDatabase.PlaceholderAvatar,
                    NPCNameID.Andy,
                    "An ambitious programmer, who is very focused in his goals and his future. He values friendship, but, he chooses who he trusts. He doesn’t like deceit that he is very sensitive due to his observant nature.",
                    new Statistics(40, 68, 97, 81),
                    new ItemsNeeded[2] {
						new ItemsNeeded(ItemNameID.USB), new ItemsNeeded(ItemNameID.GrassJellyMilkTea)
					},
                    new NPCDialogue[10] {

					# region Dialogue 1
						new NPCDialogue(
							new DialogueData[1] {
								new DialogueData("… /observes/", new DialogueButton[2] {
									new DialogueButton("Uh, hi. Um, is there something wrong with my face?", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3] {
										new DialogueData("Nothing really."),
                                        new DialogueData(" Sorry if I managed to stare at you."),
                                        new DialogueData("I was just looking around while waiting for somebody")
									}),
									new DialogueButton("You shouldn’t stare at other people like that.", 6, ButtonType.Wrong, ToughnessLevel.Level1, new DialogueData[2] {
										new DialogueData("I wasn’t staring exactly at you. I was looking around."),
                                        new DialogueData("So assuming...")
									})
								})
							}
						),
					# endregion Dialogue 1

					# region Dialogue 2
						new NPCDialogue(
							new DialogueData[1] {
                                new DialogueData("Not to be rude but, you’re quite different from before.", new DialogueButton[2] {
									new DialogueButton("Oh, I didn’t think anyone would even notice someone like me before.", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
										new DialogueData("That’s probably just me."),
										new DialogueData("I tend to observe around when I’m not interested with the class."),
										new DialogueData("And we have the same class last Tuesday, you’ll probably see me with a kid-looking girl if you didn’t see me."),
                                        new DialogueData("That’s why I recognize you.")
									}),
									new DialogueButton("Dude, that's a bit creepy. I know you observe in class, but, not everybody should be that sharp.", 7, ButtonType.Wrong, ToughnessLevel.Level1, new DialogueData[5] {
										new DialogueData("I'm sorry for noticing too many things then."),
                                        new DialogueData("If you're hiding something that has something to do with what I said."),
                                        new DialogueData("You didn't have to go say I was creepy."),
                                        new DialogueData("Meh, I shouldn't have even bothered."),
                                        new DialogueData("Such a waste of time.")
									})
								})
							}
						),
					# endregion Dialogue 2

					# region Dialogue 3
						new NPCDialogue(
							new DialogueData[1] {
								new DialogueData("It's you again. What's up?", new DialogueButton[2] {
									new DialogueButton("Oh you know, contemplating about something like maybe... joining the game jam?", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3] {
										new DialogueData("Join it."),
                                        new DialogueData("I'm not forcing you to but, I'm recommending you to join it."),
                                        new DialogueData("Just saying~ hahaha")
									}),
									new DialogueButton("Uh, the clouds? Erm, the ceiling?", 8, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[7] {
										new DialogueData("Okaaaay."),
                                        new DialogueData("Don't...try that again."),
                                        new DialogueData("One, if you want to joke, use something new."),
                                        new DialogueData("Two, if you want to joke, choose the right timing please"),
                                        new DialogueData("And three, if you want to joke something like that, be sure to use only ONE answer,"),
                                        new DialogueData("Don't doubt your punchline."),
                                        new DialogueData("I'm no comedian, but, at least I know that wasn't funny.")
									})
								})

						}),
					# endregion Dialogue 3

					# region Dialogue 4
						new NPCDialogue(new DialogueData[2] {
                            new DialogueData("I'm a bit stuck with something right now."),
                            new DialogueData("Are you any good with concepts or ideas at least?", new DialogueButton[3] {
								new DialogueButton("I guess I can say I'm a little bit above average with those kinds of things.", 5, ButtonType.Correct, new DialogueData[5] {
									new DialogueData("That's great!"),
                                    new DialogueData("I really need some help over creating a game."),
                                    new DialogueData("Not that big of a game though."),
                                    new DialogueData("Something to be created within a span of one night"),
                                    new DialogueData("I know it sounds ridiculous but, I really need an idea.")
								}),
                                new DialogueButton("What is it for?", 5, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5] {
									new DialogueData("Oh I just wanted to create a very simple game for myself."),
									new DialogueData("Just a good practice to keep my programming skills sharp."),
                                    new DialogueData("I don't usually ask for others since I have usually have a friend designer."),
                                    new DialogueData("But, well... something happened. Anyways, she isn't supposed to know this so..."),
                                    new DialogueData("I need your help, if you don't mind.")
								}),
								new DialogueButton("Hehe~ Do I get paid? They are MY ideas after all.", 9, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[5] {
									new DialogueData("I don't mind paying but..."),
                                    new DialogueData("YOU don't get ahead with the client"),
                                    new DialogueData("Because, it turns them off and might not want to let you do the work anymore."),
                                    new DialogueData("Like what I'm about to do now."),
                                    new DialogueData("So yeah, I'll look for someone else to help me instead.")
								})
							})
						}),
					# endregion Dialogue 4

					# region Dialogue 5
						new NPCDialogue(new DialogueData[5] {
							new DialogueData("Have you ever had this condition where you work better in a specific environment?"),
                            new DialogueData("I mean like the place and the time of work...?"),
                            new DialogueData("Because, I can't help but wonder if I really should stay overnight in the school"),
                            new DialogueData("if I work better when I'm at home during the night."),
                            new DialogueData("Oh wait, nevermind. I suddenly remembered that one rule." , new DialogueButton[3] {
								new DialogueButton("What one rule?", 10, ButtonType.Correct, new Statistics(10, 0), new DialogueData[2] {
									new DialogueData("It's the rule where if one decides not to stay overnight and just go home"),
                                    new DialogueData("That person shouldn't work on the game at home and instead come back the next day to work on it.")
								}),
                                new DialogueButton("I never really noticed things like that, like with the environment and all that stuff", 10, ButtonType.Correct, new Statistics(10, 0), new DialogueData[8] {
									new DialogueData("Well, after the game jam, you should find out for yourself."),
                                    new DialogueData("It'll help you with scheduling your work and all that."),
                                    new DialogueData("Like what I do..."),
                                    new DialogueData("Since I don't really work well in the mornings and a bit of the afternoon"),
                                    new DialogueData("I put all my freetime there and just relax or maybe sleep and work at night until 5 am"),
                                    new DialogueData("I also don't really work well when I'm in an overnight or"),
                                    new DialogueData("if there's some kind of disturbances around me, because I can't focus."),
                                    new DialogueData("That's why I like working in my room at home.")
								}),
								new DialogueButton("Not really. I don't care as much at all.", 10, ButtonType.Wrong, ToughnessLevel.Level3, new DialogueData[3] {
									new DialogueData("Well, I'll go with your flow then."),
                                    new DialogueData("Don't say I didn't try to remind you."),
                                    new DialogueData("Your choice, not my problem anymore.")
								})
							})
						}),
					# endregion Dialogue 5

					# region Dialogue 6
						new NPCDialogue(new DialogueData[3] {
                            new DialogueData("So thirsty...I could use some iced tea- ah no. milk tea..."),
							new DialogueData("Oh? You were there? Yo!"),
                            new DialogueData("Been there for awhile?", new DialogueButton[3]{
								new DialogueButton("Not really, just got here. Wanna grab something to drink?", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3] {
									new DialogueData("Nah, I'll pass!"),
                                    new DialogueData("As much as I want to buy a drink, I have to wait here for the rest of the crew to come."),
                                    new DialogueData("I'm waiting for about 5 people since the meeting spot's here, haha!")
								}),
								new DialogueButton("Hey! I'm not some thin air!", 7, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[4] {
									new DialogueData("Easy man!"),
									new DialogueData("Are you mad? I was just too thirsty to notice you."),
	                                new DialogueData("Chill. Didn't mean to offend you."),
                                    new DialogueData("Sometimes it's so annoying that you get offended with small stuff like that.")
								}),
                                new DialogueButton("Maybe.", 7, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[3] {
									new DialogueData("That's just creepy. I mean I noticed you staring and all"),
                                    new DialogueData("You're creeping me out a bit..."),
                                    new DialogueData("As much as I want to leave, but I'm waiting for some people.")
								})
							})
						}),
					# endregion Dialogue 6

					# region Dialogue 7
						new NPCDialogue(new DialogueData[1] {
							new DialogueData("Really? Like REALLY?" , new DialogueButton[3] {
								new DialogueButton("Someone needs to calm down.", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[8] {
									new DialogueData("Ugh, I'm trying to be."),
                                    new DialogueData("Just... wait."),
                                    new DialogueData("..."),
                                    new DialogueData("I'm a bit fine now. "),
                                    new DialogueData("I can put on my business smile now, too. Haha!"),
                                    new DialogueData("Something personal happened so..."),
                                    new DialogueData("I don't think I'm obligated to tell you."),
                                    new DialogueData("It's just something small. I'll be fiiiiine.")
								}),
								new DialogueButton("What's wrong with you?", 8, ButtonType.Wrong, ToughnessLevel.Level3, new DialogueData[2] {
									new DialogueData("Nothing."),
                                    new DialogueData("Don't ask. You're not involved so just go.")
								}),
                                new DialogueButton("You look awfully...mad...at something.", 8, ButtonType.Wrong, ToughnessLevel.Level3, new DialogueData[4] {
									new DialogueData("No no, I'm not mad."),
                                    new DialogueData("NOT. AT. ALL."),
                                    new DialogueData("Be more sensitive and just leave me alone."),
                                    new DialogueData("Unless you want to add up to my stress and irritation.")
								})
							})
						}),
					# endregion Dialogue 7

					# region Dialogue 8
						new NPCDialogue(new DialogueData[3] {
							new DialogueData("I could've sworn I fell over there."),
                            new DialogueData("Maybe it fell off in a wrong place?"),
                            new DialogueData("I just hope it's not lost forever. I have a lot of important files in there...", new DialogueButton[3] {
								new DialogueButton("How about the lost and found?" +
                                                    "Maybe someone already saw them and left it in their care.", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3]{
                                    new DialogueData("Good idea! I haven't checked there yet."),
                                    new DialogueData("Thanks a lot, man!"),
                                    new DialogueData("I owe you.")
                                }),
								new DialogueButton("You should've created some backup files for them.", 9, ButtonType.Wrong, ToughnessLevel.Level4, new DialogueData[3]{
                                    new DialogueData("That WAS the backup"),
                                    new DialogueData("I just don't want anyone else to have an access to them"),
                                    new DialogueData("If you'll excuse me, I have other better things to do than talk to you.")
                                }),
                                new DialogueButton("Maybe you were using your tongue and not your eyes to look for it?", 9, ButtonType.Wrong, ToughnessLevel.Level4, new DialogueData[4]{
                                    new DialogueData("I've been searching for it EVERYWHERE."),
                                    new DialogueData("Don't suggest that I'm not looking with my eyes!"),
                                    new DialogueData("Ugh! Nevermind. Arguing with you is not helping me find my USB."),
                                    new DialogueData("I'm leaving. You're wasting my precious time from the search.")
                                })
							})
						}),
					# endregion Dialogue 8

					# region Dialogue 9
						new NPCDialogue(new DialogueData[2] {
							new DialogueData("I hate bringing my laptop from home."),
                            new DialogueData("It's one of the reasons why I have second thoughts on the overnight during the event", new DialogueButton[3] {
								new DialogueButton("Why? It's not as hassle as a desktop right? One backpack and you're good to go.", 5, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3]{
                                    new DialogueData("It might not be the same hassle level as a desktop."),
                                    new DialogueData("But, it's still hassle, because I have to remove the wires and mess with my setup at home."),
                                    new DialogueData("When I come back after the event, I have to wire it up again and setup it up with my second monitor.")
                                }),
								new DialogueButton("If it's such a hassle, then don't go anymore. It's not required to overnight anyways.", 10, ButtonType.Wrong, ToughnessLevel.Level5, new DialogueData[4]{
                                    new DialogueData("Do you want to make my teammates lose?"),
                                    new DialogueData("I need as much time as I can to finish the program!"),
                                    new DialogueData("And I can't program efficiently when I'm time pressured!"),
                                    new DialogueData("So please, don't suggest something like that to me anymore.")
                                }),
                                new DialogueButton("Don't participate then if you can't sacrifice.", 10, ButtonType.Wrong, ToughnessLevel.Level5, new DialogueData[4]{
                                    new DialogueData("I can sacrifice thank you very much!"),
                                    new DialogueData("You don't tell me what to do, okay?"),
                                    new DialogueData("You know what? Don't butt-in my business anymore and perhaps..."),
                                    new DialogueData("It's time for you to go away?")
                                })
							})
						}),
					# endregion Dialogue 9

					# region Dialogue 10

                        // level6 mini game pls
						new NPCDialogue(new DialogueData[3] {
							new DialogueData("I'm sure that kid's getting hyped up over and over again."),
                            new DialogueData("Hopefully she doesn't annoy or weird out more people."),
                            new DialogueData("The only relieving thing she does is that she's prepared like a good little girl scout she is.", new DialogueButton[3] {
								new DialogueButton("What kid?", ButtonType.Correct, new Statistics(10, 0), new DialogueData[5]{
                                    new DialogueData("She's not actually a kid."),
                                    new DialogueData("She's in the same age group as us. She's a designer but, sometimes,"),
                                    new DialogueData("she has to play other roles at the same time too if needed."),
                                    new DialogueData("Even if that's the case though, I don't get how she's so hyped up for the event."),
                                    new DialogueData("She's so hyped up that I don't even know if it's normal or is it just really me being too calm?")
                                }),
                                new DialogueButton("Are you prepared yourself?", ButtonType.Correct, new Statistics(10, 0), new DialogueData[7]{
                                    new DialogueData("I am actually, but, I don't have to jump up and down to show that."),
                                    new DialogueData("How can she be so talkative when the topic's in her interest and at the same time"),
                                    new DialogueData("shy... when talking to strangers."),
                                    new DialogueData("Anyways, hmm, I have my stuff ready and packed. Check."),
                                    new DialogueData("I have snacks? I guess. And..."),
                                    new DialogueData("The softwares I need are already in my laptop so...Check."),
                                    new DialogueData("I guess I am ready. Hahaha! Just double-checking.")
                                }),
								new DialogueButton("If she only has a few good points for you and more of the wrongs, then shouldn't you stop being friends with her?", ButtonType.Wrong, ToughnessLevel.Level5, new DialogueData[5]{
                                    new DialogueData("If you're only looking for the good points in people"),
                                    new DialogueData("And that's the only basis you have to look at when looking for a friend,"),
                                    new DialogueData("Then, you don't deserve the friends you have now!"),
                                    new DialogueData("Other than that, please refrain from telling me to stop being friends with her."),
                                    new DialogueData("You don't have the right to!")
                                })
							})
						}),
					# endregion Dialogue 10

					},
					new SympathyText(
						new string[1] { "Sorry" }
					),
					new AcceptedText[1] {
						new AcceptedText(
						new DialogueData[1] { 
							new DialogueData("Thank you for giving me ") 
						})
					},
					new DeclinedText[1] {
						new DeclinedText(
						new DialogueData[1] { 
							new DialogueData("Sorry I don't need this") 
						})
					});
                # endregion Andy

                # region Jenevieve
                npcDataInfoList[4] = new NPCData(
                    "Jenevieve Ashworth",
                    TextureResource.AvatarDatabase.PlaceholderAvatar,
                    NPCNameID.Jenevieve,
                    "A determined 3D Artist, who wants to improve herself in the specific specialty of 3D Art. She is slowly building herself up and is also willing to learn from other people.",
                    new Statistics(87, 76, 34, 72),
                    new ItemsNeeded[2] {
						new ItemsNeeded(ItemNameID.WhiteEarphones), new ItemsNeeded(ItemNameID.IcedTea)
					},
                    new NPCDialogue[10] {

					# region Dialogue 1
						new NPCDialogue(
							new DialogueData[1] {
								new DialogueData("Hey. Have you seen any earphones around?", new DialogueButton[2] {
									new DialogueButton("Um, I don’t think so. I only arrived here a few minutes ago, you see.", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5] {
										new DialogueData("Dammit. Well, if you somehow find it..hmm..."),
                                        new DialogueData("Ah, if you find it, just give it to me when you see me next time?"),
                                        new DialogueData("You're in the same course as me, right?"),
										new DialogueData("Erm, or you can just give it to the lost and found and I'll retrieve it."),
                                        new DialogueData("Just tell the lost and found that I own it. My name's Jenevieve by the way.")
									}),
									new DialogueButton("Haven’t seen one. Try somewhere else.", 6, ButtonType.Wrong, ToughnessLevel.Level1, new DialogueData[3] {
										new DialogueData("You don't have to be so rude."),
                                        new DialogueData("I was just asking."),
                                        new DialogueData("Much bad vibes, how annoying.")
									})
								})
							}
						),
					# endregion Dialogue 1

					# region Dialogue 2
						new NPCDialogue(
							new DialogueData[3] {
                                new DialogueData("Oh, I give up!"),
                                new DialogueData("I’ll check it out at the lost and found later."),
								new DialogueData("Come to think of it. I’ve never seen you around before. Who’re you?", new DialogueButton[2] {
									new DialogueButton("Someone not that important, but been around for some time. Just call me Hills!", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3] {
										new DialogueData("I can't seem to grab a hold of what you meant"),
										new DialogueData("Are you depressed or is this normally you?"),
										new DialogueData("Anyways, nice meeting you..uhh Hills.")
									}),
									new DialogueButton("I'm busy.", 7, ButtonType.Wrong, ToughnessLevel.Level1, new DialogueData[3] {
										new DialogueData("Really? Just...really?"),
										new DialogueData("You know what? Nevermind. "),
                                        new DialogueData("Geez, I was only trying to be friendly.")
									})
								})
							}
						),
					# endregion Dialogue 2

					# region Dialogue 3
						new NPCDialogue(
							new DialogueData[3] {
								new DialogueData("It’s you again?"),
								new DialogueData("Well, I shouldn’t be surprised, right?"),
								new DialogueData("We ARE in the same course.", new DialogueButton[2] {
									new DialogueButton("I'm surprised, you even noticed me. I don't usually leave much impression on others.", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
										new DialogueData("Hey, I remembered you."),
										new DialogueData("So, don't be so down."),
                                        new DialogueData("I'm sure more people will recognize you sooner or later."),
                                        new DialogueData("It's not about recognition though, it's more about making friends, okay?")
									}),
									new DialogueButton("We're in the same course and we might've been in the same classes the previous terms" +
                                                        "but, I was thin air to you so don't bother.", 8, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[5] {
										new DialogueData("Sorry if I offended you or anything..."),
                                        new DialogueData("But, that way of talking doesn't sit well with me."),
                                        new DialogueData("Got that?"),
                                        new DialogueData("No wonder people didn't approach you."),
										new DialogueData("With that kind of attitude?")
									})
								})

						}),
					# endregion Dialogue 3

					# region Dialogue 4
						new NPCDialogue(new DialogueData[3] {
                            new DialogueData("Do you know any Franz, Maxine, Noelle, Andy or Bart?"),
							new DialogueData("We’re supposed to have a meet up somewhere."),
                            new DialogueData("Ugh, isn’t it too much to ask for them to reply where they are?", new DialogueButton[3] {
								new DialogueButton("Maybe you should call one of them instead?", 5, ButtonType.Correct, new DialogueData[6] {
									new DialogueData("Yeah, I could do that... if I had that much load to call them one by one."),
									new DialogueData("Thanks for the suggestion though."),
                                    new DialogueData("Oh, wait."),
                                    new DialogueData("Finally! Someone replied!"),
                                    new DialogueData("Wait, it's not a reply, it's a phonecall"),
                                    new DialogueData("I gotta go answer this. See you!")
								}),
                                new DialogueButton("You should just go straight to your meet up place." +
                                                    "They could be there already.", 5, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5] {
									new DialogueData("But what if they're not there?"),
									new DialogueData("Wait, why am I having second thoughts?"),
                                    new DialogueData("Of course, Noelle and Andy are gonna be there."),
                                    new DialogueData("They're always early."),
                                    new DialogueData("Guess this means, I gotta jet. See you around!")
								}),
								new DialogueButton("Familiar names, but no. Now, go along and find your friends.", 9, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[1] {
									new DialogueData("Jerk.")
								})
							})
						}),
					# endregion Dialogue 4

					# region Dialogue 5
						new NPCDialogue(new DialogueData[1] {
							new DialogueData("Are you participating?", new DialogueButton[3] {
								new DialogueButton("Yeah, I am! I’m kind of excited and at the same time sad about it," +
                                                    "‘cause it’s my first and last time participating as a student!", 10, ButtonType.Correct, new Statistics(10, 0), new DialogueData[7] {
									new DialogueData("Oh, you didn’t participate in the previous years?"),
                                    new DialogueData("I don’t know if it would be fun for you or not."),
                                    new DialogueData("I had fun for spending time and moments with friends since they were my teammates back then."),
                                    new DialogueData("It was tiring as hell though!"),
                                    new DialogueData("Trying to stay awake for how many hours for 3 days and two nights straight?"),
                                    new DialogueData("I don’t even know if I want a repeat of that again."),
                                    new DialogueData("Hmm, it wasn’t that bad at the least, so, you might see me there? I guess?")
								}),
                                new DialogueButton("Yeah, I guess I'm a bit prepared for it. Maybe.", 10, ButtonType.Correct, new Statistics(10, 0), new DialogueData[6] {
									new DialogueData("It's better than nothing right?"),
                                    new DialogueData("If you prepared at least something, you have an advantage already"),
                                    new DialogueData("So don't let nervousness get to you, alright? Calm down."),
                                    new DialogueData("Though, I can't say the same for myself."),
                                    new DialogueData("I'm nervous too, but at the same time, kind of excited."),
                                    new DialogueData("I wonder what game I'll be able to create with the others for this year...")
								}),
								new DialogueButton("In what?", 10, ButtonType.Wrong, ToughnessLevel.Level3, new DialogueData[2] {
									new DialogueData("In the game jam, duh?"),	
                                    new DialogueData("How could you not know that?")
								})
							})
						}),
					# endregion Dialogue 5

					# region Dialogue 6
						new NPCDialogue(new DialogueData[6] {
                            new DialogueData("Ugh, I could use some noodles right now..."),
							new DialogueData("No.. no. I want some green mangoes"),
                            new DialogueData("Or maybe some iced tea?"),
                            new DialogueData("Ah, I suddenly remembered last night's tv show! Darn it!"),
                            new DialogueData("Now, I want some burger or steak!"),
                            new DialogueData("So frustrating...!", new DialogueButton[3]{
								new DialogueButton("How come you have those cravings? It's a lot and " +
                                                    "you don't look like you get fat and stuff.", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
									new DialogueData("Don't mind me."),
									new DialogueData("I'm just being my normal self haha!"),
									new DialogueData("And well, I have something of a personal thing going on"),
                                    new DialogueData("I can't really tell you, right now.")
								}),
								new DialogueButton("Are you pregnant or something? That's a lot of cravings!", 7, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[4] {
									new DialogueData("What?!"),
									new DialogueData("You don't ask a woman a question like that."),
	                                new DialogueData("Just because I craved a little...YES a LITTLE, it doesn't automatically mean I'm pregnant!"),
                                    new DialogueData("Ugh! How annoying.")
								}),
                                new DialogueButton("Are you buying for all of those? Can you even finish all of those?", 7, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[6] {
									new DialogueData("Well, duh! I won't buy for all of those."),
                                    new DialogueData("And of course, I can't finish all of those if I had them"),
                                    new DialogueData("That's just my tongue and stomach talking."),
                                    new DialogueData("Also, it's not like I have that much money on me."),
                                    new DialogueData("Obviously, that's one of the reasons why I'm frustrated."),
                                    new DialogueData("I don't have enough money for any of it!")
								})
							})
						}),
					# endregion Dialogue 6

					# region Dialogue 7
						new NPCDialogue(new DialogueData[1] {
							new DialogueData("You hungry?", new DialogueButton[3] {
								new DialogueButton("Just a little. I guess? Hm, here have some, if you like some orange juice...", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[2] {
									new DialogueData("Thanks for the hospitality, but, I have to go though."),
                                    new DialogueData("Ah and you make a nice natural orange juice.")
								}),
								new DialogueButton("Thanks, but no thanks.", 8, ButtonType.Wrong, ToughnessLevel.Level3, new DialogueData[4] {
									new DialogueData("Don't force your stomach to go quiet!"),
                                    new DialogueData("If you're hungry then you should eat okay?"),
                                    new DialogueData("I'm not the one to talk though."),
                                    new DialogueData("I usually skip my breakfast.")
								}),
                                new DialogueButton("What could be more important than food?", 8, ButtonType.Wrong, ToughnessLevel.Level3, new DialogueData[3] {
									new DialogueData("I don't know. Money? Family?"),
                                    new DialogueData("Obviously, a lot of things are!"),
                                    new DialogueData("Don't ask about obvious things, please!")
								})
							})
						}),
					# endregion Dialogue 7

					# region Dialogue 8
						new NPCDialogue(new DialogueData[3] {
							new DialogueData("You don't have any class, do you?"),
                            new DialogueData("From morning until now, I only saw you doing one thing."),
                            new DialogueData("Over and over.", new DialogueButton[3] {
								new DialogueButton("I don't have any actually. Does that strike odd to you?", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[2]{
                                    new DialogueData("No, not really."),
                                    new DialogueData("I'm just surprised you can last this long in school doing nothing.")
                                }),
								new DialogueButton("You think?", 9, ButtonType.Wrong, ToughnessLevel.Level4, new DialogueData[3]{
                                    new DialogueData("I was just pointing out a fact."),
                                    new DialogueData("You don't have to be mad!"),
                                    new DialogueData("Geez!")
                                }),
                                new DialogueButton("You're accusing me of stalking, aren't you?", 9, ButtonType.Wrong, ToughnessLevel.Level4, new DialogueData[2]{
                                    new DialogueData("No, I don't."),
                                    new DialogueData("Seriously, I was just asking!")
                                })
							})
						}),
					# endregion Dialogue 8

					# region Dialogue 9
						new NPCDialogue(new DialogueData[2] {
							new DialogueData("Sigh, just thinking about it makes my back ache."),
                            new DialogueData("Such a hassle.", new DialogueButton[3] {
								new DialogueButton("What's a hassle?", 5, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4]{
                                    new DialogueData("Oh you know, bringing the laptop and at the same time"),
                                    new DialogueData("...bringing your clothes."),
                                    new DialogueData("It's only one back pack, but, it's too heavy!"),
                                    new DialogueData("But, it's one of the things I figure I have to endure if I want to participate and enjoy the jam.")
                                }),
								new DialogueButton("If it's such a hassle, why do it?", 10, ButtonType.Wrong, ToughnessLevel.Level5, new DialogueData[1]{
                                    new DialogueData("Didn't you even wonder what I was doing and why I was doing it?")
                                }),
                                new DialogueButton("Then, don't think about it! It's not like you're doing it right now.", 10, ButtonType.Wrong, ToughnessLevel.Level5, new DialogueData[2]{
                                    new DialogueData("Even if I don't, I'll still experience it anyway, obviously"),
                                    new DialogueData("Sigh.")
                                })
							})
						}),
					# endregion Dialogue 9

					# region Dialogue 10
                        // level6 mini game pls
						new NPCDialogue(new DialogueData[4] {
							new DialogueData("Since you're joining the game jam, are you more of an artist or a programmer?"),
                            new DialogueData("I'm an artist but I lean more on 3D."),
                            new DialogueData("I'm not recruiting or anything for a premade team, don't misunderstand"),
                            new DialogueData("It's just a simple question.", new DialogueButton[3] {
								new DialogueButton("If that's the case then, I'm more of a programmer.", ButtonType.Correct, new Statistics(10, 0), new DialogueData[5]{
                                    new DialogueData("That's new. I can't believe there's another programmer in our batch"),
                                    new DialogueData("I only know a few."),
                                    new DialogueData("Programmers are quite rare, not during the game jam though."),
                                    new DialogueData("There are other schools and game industry practitioners participating after all."),
                                    new DialogueData("It's not going to be easy, but, hey... experience right?")
                                }),
                                new DialogueButton("If that's the case then, I'm more of an artist.", ButtonType.Correct, new Statistics(10, 0), new DialogueData[2]{
                                    new DialogueData("Oh the same as me then!"),
                                    new DialogueData("Welcome aboard to the game jam artists' crew hahaha!")
                                }),
								new DialogueButton("I'm flexible...I think? Hehe", ButtonType.Wrong, ToughnessLevel.Level5, new DialogueData[3]{
                                    new DialogueData("Please! Be more specific!"),
                                    new DialogueData("Ugh, 'cause if you don't know what role you are, you won't know what you should contribute."),
                                    new DialogueData("Have mercy on your teammates! There's time pressure you know that right?")
                                })
							})
						}),
					# endregion Dialogue 10

					},
					new SympathyText(
						new string[1] { "Sorry" }
					),
					new AcceptedText[1] {
						new AcceptedText(
						new DialogueData[1] { 
							new DialogueData("Thank you for giving me ") 
						})
					},
					new DeclinedText[1] {
						new DeclinedText(
						new DialogueData[1] { 
							new DialogueData("Sorry I don't need this") 
						})
					});
                # endregion Jenevieve

                # region Franz
                npcDataInfoList[5] = new NPCData(
                    "Franzine 'Franz' Vespermann",
                    TextureResource.AvatarDatabase.PlaceholderAvatar,
                    NPCNameID.Franz,
                    "A creative 2D and 3D Artist, who is willing to learn and self-study to improve her skills. She has this certain work flow that she doesn’t want disturbed or else she procrastinates and ends up cramming.",
                    new Statistics(90, 77, 30, 65),
                    new ItemsNeeded[2] {
						new ItemsNeeded(ItemNameID.Markers), new ItemsNeeded(ItemNameID.OrangeJuice)
					},
                    new NPCDialogue[10] {

					# region Dialogue 1
						new NPCDialogue(
							new DialogueData[1] {
								new DialogueData("Hi… I’m Franzine, but, um… uh, you can just call me Franz.", new DialogueButton[2] {
									new DialogueButton("H-Hi! Um…", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3] {
										new DialogueData("Are you nervous? Don’t be!"),
                                        new DialogueData("I-I’ll get nervous too."),
                                        new DialogueData("It's nice to meet you though!")
									}),
									new DialogueButton("Cool you have a nickname!", 6, ButtonType.Wrong, ToughnessLevel.Level1, new DialogueData[2] {
										new DialogueData("Franzine's a long name so...yeah"),
                                        new DialogueData("That's basically it though.")
									})
								})
							}
						),
					# endregion Dialogue 1

					# region Dialogue 2
						new NPCDialogue(
							new DialogueData[1] {
                                new DialogueData("A-Are you an artist or…?", new DialogueButton[2] {
									new DialogueButton("I’m a bit of both but, I’m more artsy.", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3] {
										new DialogueData("Um, do you like manga and anime?"),
                                        new DialogueData("I'm not assuming that all artists have to like them."),
                                        new DialogueData("I.. just wanna ask. Hehe")
									}),
									new DialogueButton("I don’t know.", 7, ButtonType.Wrong, ToughnessLevel.Level1, new DialogueData[6] {
										new DialogueData("Oh so you're not sure yourself yet then?"),
                                        new DialogueData("That's a bit worrisome..."),
                                        new DialogueData("You should decide soon."),
                                        new DialogueData("Just a little piece of advice...um.. I'm not telling you to actually do this."),
                                        new DialogueData("This is just an advice, but, if you have tried every role already available."),
                                        new DialogueData("Then why not be in the role you enjoyed the most in, right?")
									})
								})
							}
						),
					# endregion Dialogue 2

					# region Dialogue 3
						new NPCDialogue(
							new DialogueData[5] {
                                new DialogueData("Have you tried using watercolors and markers before?"),
                                new DialogueData("It’s really fun and interesting with what you can do with those!"),
                                new DialogueData("I suggest you try a lot of things from the traditional side of the spectrum if you’re tired for doing digital for awhile."),
                                new DialogueData("I mean don’t just sketch and color with pencils. I-I’m not forcing you or anything."),
								new DialogueData("It’s just a suggestion.", new DialogueButton[2] {
									new DialogueButton("That sounds like a cool suggestion. I just might try it!", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
										new DialogueData("Do prepare to spend a lot though."),
                                        new DialogueData("The prices for different art sets like markers, color pencils, water brushes and watercolor paint are no joke..."),
                                        new DialogueData("especially when you have to constantly refill some of them."),
                                        new DialogueData("It’s worth it though!")
									}),
									new DialogueButton("Traditional's not that good. It's kind of lame." +
                                                        "I'll do art with digital any day", 8, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[3] {
										new DialogueData("It's your opinion. I can't do anything about it."),
                                        new DialogueData("Just wanted to broaden your horizons, but if you don't want any of it..."),
                                        new DialogueData("then okay.")
									})
								})

						}),
					# endregion Dialogue 3

					# region Dialogue 4
						new NPCDialogue(new DialogueData[4] {
                            new DialogueData("Speaking of markers…"),
                            new DialogueData("I can’t seem to find my set."),
                            new DialogueData("I really didn’t spend anything for those but, all the more reason to find them."),
                            new DialogueData("They’re gifts from friends of mine.", new DialogueButton[3] {
								new DialogueButton("Try tracing back your steps. Remember what you did today and where you’ve done those things.", 5, ButtonType.Correct, new DialogueData[3] {
									new DialogueData("I can't remember everything but, I'll do my best."),
                                    new DialogueData("I mean I have to...if I want to get my markers back."),
                                    new DialogueData("Thanks for giving me an idea on what to besides just blindly looking for it.")
								}),
                                new DialogueButton("Try in the lost and found?", 5, ButtonType.Correct, new Statistics(10, 0), new DialogueData[2] {
									new DialogueData("Hopefully, the one who found my markers is a person of good intentions."),
                                    new DialogueData("And hopefully, that person truly gave it to the lost and found in a complete set.")
								}),
								new DialogueButton("If it's a set of markers, then you might not find them again." + 
                                                    "They're expensive soooo, good luck with that", 9, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[3] {
									new DialogueData("We should stay positive!"),
                                    new DialogueData("Why so negative? There's always a way!"),
                                    new DialogueData("Stop making me lose my spirit.")
								})
							})
						}),
					# endregion Dialogue 4

					# region Dialogue 5
						new NPCDialogue(new DialogueData[1] {
							new DialogueData("Game Jam's around the corner. I'm a bit anxious." , new DialogueButton[3] {
								new DialogueButton("Why?", 10, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
									new DialogueData("Despite that it's fun and all, it IS still a competition."),
                                    new DialogueData("That doesn't matter to me much though, I'm more concerned with my skills."),
                                    new DialogueData("I mean I prepared by practicing lots of times."),
                                    new DialogueData("I'm just scared that it might not have an effect.")
								}),
                                new DialogueButton("Don't be! It might get to you. Stay cheerful and let's all do our best!", 10, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
									new DialogueData("That sounds a bit relieving."),
                                    new DialogueData("The anxiousness is still there but, it's not as bad as it was before."),
                                    new DialogueData("Thanks..!"),
                                    new DialogueData("Now to prepare on how to talk to new people.")
								}),
								new DialogueButton("Everyone is not just you. So stop complaining.", 10, ButtonType.Wrong, ToughnessLevel.Level3, new DialogueData[4] {
									new DialogueData("What?"),
                                    new DialogueData("I was just telling you how I feel. I'm not complaining anything."),
                                    new DialogueData("If you don't like me sharing things with you, you could just tell me!"),
                                    new DialogueData("I thought we were friends enough to do at least that.")
								})
							})
						}),
					# endregion Dialogue 5

					# region Dialogue 6
						new NPCDialogue(new DialogueData[2] {
                            new DialogueData("Yes! It updated and I finally read it."),
                            new DialogueData("Gosh, those moments. Hehehe~", new DialogueButton[3]{
								new DialogueButton("You seem to be enjoying that to be laughing in your own little world.", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
									new DialogueData("I just can't help it. The updated chapter was just so sweet."),
                                    new DialogueData("You should try reading it. Well, if you're interesed in shoujo mangas"),
                                    new DialogueData("The art's so pretty though especially when it was animated."),
                                    new DialogueData("The colors and the likes. So clean, it's a pastel-colored artwork painted using water color.")
								}),
								new DialogueButton("Did someone die already?", 7, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[3] {
									new DialogueData("No one's dying."),
                                    new DialogueData("No character death in this story, okay?"),
                                    new DialogueData("Not that I can control the story...STILL! I just know it has no character death!")
								}),
                                new DialogueButton("Get over it. You do know that you have to wait again for the next update right?", 7, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[4] {
									new DialogueData("STOP IT!"),
                                    new DialogueData("Stop being so antagonistic!"),
                                    new DialogueData("What did I ever do to you?"),
                                    new DialogueData("I just want some peace and enjoy my manga like everyone else...")
								})
							})
						}),
					# endregion Dialogue 6

					# region Dialogue 7
						new NPCDialogue(new DialogueData[3] {
							new DialogueData("Even if I'm an artist, it's really difficult to think of designs myself."),
                            new DialogueData("Sure there are a lot of reference materials that could be used, but I want to draw something unique"),
                            new DialogueData("that I imagined intricately in my head myself and not imagined and designed by others.", new DialogueButton[3] {
								new DialogueButton("Why don't you try reading books or anything without pictures but with a story?", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5] {
									new DialogueData("I'm not really the hardcore reading type of person."),
                                    new DialogueData("I just read books every now and then if they're interesting enough."),
                                    new DialogueData("But if that would help me like picture out a lot of things on top of my head,"),
                                    new DialogueData("I guess, why not? And maybe after reading, I can draw up some scenarios from the book"),
                                    new DialogueData("and design the characters myself!")
								}),
								new DialogueButton("Why are you even an artist if you can't even think of the concept on what you'd like to draw?", 8, ButtonType.Wrong, ToughnessLevel.Level3, new DialogueData[4] {
									new DialogueData("Hey!"),
                                    new DialogueData("That's crossing the line."),
                                    new DialogueData("I'm doing my best to improve on that you know?"),
                                    new DialogueData("I work hard for that, okay?!")
								}),
                                new DialogueButton("I wonder if you'd even get clients if they knew you always have art blocks like that.", 8, ButtonType.Wrong, ToughnessLevel.Level3, new DialogueData[3] {
									new DialogueData("You're not helping and you're just pushing me down!"),
                                    new DialogueData("Please. I don't need those kind of words from you."),
                                    new DialogueData("I'm trying to lessen those art blocks so don't imply that I've been doing nothing about it!")
								})
							})
						}),
					# endregion Dialogue 7

					# region Dialogue 8
						new NPCDialogue(new DialogueData[2] {
							new DialogueData("There's perspective, conceptualizing..."),
                            new DialogueData("There's also shading and detailing...", new DialogueButton[3] {
								new DialogueButton("Just what... are you talking about?", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5]{
                                    new DialogueData("Ah! Y-You surprised me!"),
                                    new DialogueData("I didn't see you there!"),
                                    new DialogueData("Well, if you're talking about the things I've been listing down in my head"),
                                    new DialogueData("They're things I want to try to improve on."),
                                    new DialogueData("I still have a long way to go I guess, hahaha")
                                }),
								new DialogueButton("Are you sure you can master all of those? That's a lot!", 9, ButtonType.Wrong, ToughnessLevel.Level4, new DialogueData[4]{
                                    new DialogueData("I can manage."),
                                    new DialogueData("Don't you look down on me!"),
                                    new DialogueData("It doesn't help with my morale if you keep on badgering and looking down on me."),
                                    new DialogueData("So please stop bothering me...and maybe, go away?")
                                }),
                                new DialogueButton("There's also the mastery of your use of the softwares if you'll do it digitally.", 9, ButtonType.Wrong, ToughnessLevel.Level4, new DialogueData[3]{
                                    new DialogueData("I know that!"),
                                    new DialogueData("And I can manage okay?"),
                                    new DialogueData("And yes you were implying something that puts me down.")
                                })
							})
						}),
					# endregion Dialogue 8

					# region Dialogue 9
						new NPCDialogue(new DialogueData[1] {
							new DialogueData("It's a good thing that my place is nearby the school.", new DialogueButton[3] {
								new DialogueButton("Why is that a good thing?", 5, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3]{
                                    new DialogueData("I don't have to be worried constantly for a long duration when I travel."),
                                    new DialogueData("just because I'm carrying my laptop and the likes."),
                                    new DialogueData("And it's less back pain for me when the game jam time comes.")
                                }),
								new DialogueButton("You don't have to say that to make us faraway travellers envious.", 10, ButtonType.Wrong, ToughnessLevel.Level5, new DialogueData[3]{
                                    new DialogueData("T-that's not what I meant..."),
                                    new DialogueData("I'm just sharing and all..."),
                                    new DialogueData("And besides, I'm a faraway traveller too if I went home during weekends!")
                                }),
                                new DialogueButton("So what?", 10, ButtonType.Wrong, ToughnessLevel.Level5, new DialogueData[4]{
                                    new DialogueData("Huh?"),
                                    new DialogueData("Why so rude?"),
                                    new DialogueData("Ugh, I'll just leave you alone now."),
                                    new DialogueData("I was just expressing my thoughts. Hmph.")
                                })
							})
						}),
					# endregion Dialogue 9

					# region Dialogue 10

                        // level6 mini game pls
						new NPCDialogue(new DialogueData[2] {
							new DialogueData("I wonder if his laptop's going to unfortunately crash again during game jam"),
                            new DialogueData("Hopefully, it won't? That'd be a hassle for his teammates and all.", new DialogueButton[3] {
								new DialogueButton("Another laptop crash?", ButtonType.Correct, new Statistics(10, 0), new DialogueData[4]{
                                    new DialogueData("Oh, you were listening in?"),
                                    new DialogueData("Um, it's just that he just had his laptop get fixed and uh..."),
                                    new DialogueData("His laptop always crashes at the wrong time."),
                                    new DialogueData("And by at the wrong time, I meant when he badly needs it for school and the likes.")
                                }),
                                new DialogueButton("'His'?", ButtonType.Correct, new Statistics(10, 0), new DialogueData[4]{
                                    new DialogueData("If you're wondering who, it's Bart."),
                                    new DialogueData("If you see a bit of a chubby guy with earrings and black shirt..oh.."),
                                    new DialogueData("That's a bit of um.. generic."),
                                    new DialogueData("Sorry, I don't know how else to describe him.")
                                }),
								new DialogueButton("I'm definitely not going to be his teammate then", ButtonType.Wrong, ToughnessLevel.Level5, new DialogueData[7]{
                                    new DialogueData("Hey!"),
                                    new DialogueData("What did he ever do to you?"),
                                    new DialogueData("It was just his laptop and it's a minimal chance anyway!"),
                                    new DialogueData("Don't judge him because of his laptop."),
                                    new DialogueData("How do you look for teammates then? Just by their use to you?!"),
                                    new DialogueData("Well, if that's the case then, I'm definitely NOT going to be on your team if by chance"),
                                    new DialogueData("you invite me. Hmph.")
                                })
							})
						}),
					# endregion Dialogue 10

					},
					new SympathyText(
						new string[1] { "Sorry" }
					),
					new AcceptedText[1] {
						new AcceptedText(
						new DialogueData[1] { 
							new DialogueData("Thank you for giving me ") 
						})
					},
					new DeclinedText[1] {
						new DeclinedText(
						new DialogueData[1] { 
							new DialogueData("Sorry I don't need this") 
						})
					});
                # endregion Franz

                # region Bart
                npcDataInfoList[6] = new NPCData(
                    "Bartholomew 'Bart' Beacock",
                    TextureResource.AvatarDatabase.PlaceholderAvatar,
                    NPCNameID.Bart,
                    "A sound designer, who is very much talented with stringed instruments. Other than sound, he is very diverse. He may seem quiet, but, when you get to know him better, he’s actually funny and gives out weird trivias that you will find yourself laughing or baffled.",
                    new Statistics(85, 80, 45, 95),
                    new ItemsNeeded[2] {
						new ItemsNeeded(ItemNameID.GuitarPick), new ItemsNeeded(ItemNameID.HotCoffee)
					},
                    new NPCDialogue[10] {

					# region Dialogue 1
						new NPCDialogue(
							new DialogueData[1] {
								new DialogueData("Oi, ‘sup!", new DialogueButton[2] {
									new DialogueButton("Oh, hi there!", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
										new DialogueData("I'm Bartholomew... but yeah that's kinda long so..."),
                                        new DialogueData("Call me Bart."),
                                        new DialogueData("Um, have I seen you around before...?"),
                                        new DialogueData("Maybe in class. I guess?")
									}),
									new DialogueButton("Uh.. hi.", 6, ButtonType.Wrong, ToughnessLevel.Level1, new DialogueData[3] {
										new DialogueData("..."),
                                        new DialogueData("Uh, you're kind of...I don't know..."),
                                        new DialogueData("/scratches head/")
									})
								})
							}
						),
					# endregion Dialogue 1

					# region Dialogue 2
						new NPCDialogue(
							new DialogueData[5] {
                                new DialogueData("Just a random thought on the top of my head."),
                                new DialogueData("Have you ever heard of the Bermuda Triangle?"),
                                new DialogueData("Legend says a lot of plane crashed down there and a lot of boats sank there."),
                                new DialogueData("What if the reason for those crashes and sinking wasn’t really due to the storms and such?"),
                                new DialogueData("Didn’t you know that the lochness monster is actually true and is living in the Bermuda Triangle?" , new DialogueButton[2] {
									new DialogueButton("I know all about the Bermuda Triangle and it is true, not the lochness monster though." +
                                                    "I do my own fair research about general things", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
										new DialogueData("Oh..."),
                                        new DialogueData("You got me good, hahaha!"),
                                        new DialogueData("I guess you're another person not to joke on about those things since"),
                                        new DialogueData("you might have a rebuttal for the 'facts' I'm about to say next. hahaha!")
									}),
									new DialogueButton("What kind of nonsense are you talking about?", 7, ButtonType.Wrong, ToughnessLevel.Level1, new DialogueData[2] {
										new DialogueData("It’s not nonsense. I was only trying to inform you."),
                                        new DialogueData("Alright, alright. You don’t really care so I’ll be going now.")
									})
								})
							}
						),
					# endregion Dialogue 2

					# region Dialogue 3
						new NPCDialogue(
							new DialogueData[1] {
								new DialogueData("Knock knock!", new DialogueButton[2] {
									new DialogueButton("Pft. Who’s there?", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[2] {
										new DialogueData("Actually… I forgot what my joke was."),
                                        new DialogueData("Hahaha! Sorry, sorry.")
									}),
									new DialogueButton("I’m not riding on that joke. So please stop.", 8, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[4] {
										new DialogueData("Why are you such a killjoy?"),
                                        new DialogueData("I just want to have fun and maybe"),
                                        new DialogueData("make everyone else here laugh!"),
                                        new DialogueData("Tch. Fine. Nevermind then.")
									})
								})

						}),
					# endregion Dialogue 3

					# region Dialogue 4
						new NPCDialogue(new DialogueData[1] {
                            new DialogueData("Pfft. Hahaha did you just fart?", new DialogueButton[3] {
								new DialogueButton("If you were the one who asked first, then you’re the one who farted. Haha", 5, ButtonType.Correct, new DialogueData[2] {
									new DialogueData("That maybe true. But, you WERE quiet for some time. Haha!"),
                                    new DialogueData("Pfft. Just... hahaha! admit it!")
								}),
                                new DialogueButton("Nope, if I had been the one, it would’ve been more deadly than this. Hahaha!", 5, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
									new DialogueData("Hahaha! That's too good to be true!"),
                                    new DialogueData("You know what? You're quite funny."),
                                    new DialogueData("To be honest, I didn't know you had it in you."),
                                    new DialogueData("You just don't seem the type. hahaha!")
								}),
								new DialogueButton("Don’t frame me for something you did! What a waste of time.", 9, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[2] {
									new DialogueData("Oi, chill!"),
                                    new DialogueData("I was just kidding, man!")
								})
							})
						}),
					# endregion Dialogue 4

					# region Dialogue 5
						new NPCDialogue(new DialogueData[4] {
                            new DialogueData("I’m kind of worried about game jam"),
                            new DialogueData("Well, I’m not really worried about the competition and all."),
                            new DialogueData("I’m not even worried about the hassle of travelling with a laptop."),
							new DialogueData("What I’m worried about is that if my laptop will survive the game jam." , new DialogueButton[3] {
								new DialogueButton("Survive...that's one heavy word for the condition of your laptop.", 10, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
									new DialogueData("Well, it has that one habit of crashing, EVERY TIME I use it on something important.. "),
                                    new DialogueData("And well, game jam is important and at the same time, time pressured."),
                                    new DialogueData("So every ticking second shouldn’t be wasted with much reboots on my laptop if it ever crashes."),
                                    new DialogueData("Sigh.")
								}),
                                new DialogueButton("If you had your laptop fixed before, then I don't think it will crash anytime soon!", 10, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
									new DialogueData("I guess I just have to think positive and maybe, it won’t crash?"),
                                    new DialogueData("But just in case, I might have to do an every hour backup thing."),
                                    new DialogueData("Just in case it won’t reboot anymore because of multiple crashes."),
                                    new DialogueData("Such a tedious process.")
								}),
								new DialogueButton("Then, why join at all? You’re just going to waste your time and" +
                                                    "not enjoy creating the game if your laptop keeps crashing.", 10, ButtonType.Wrong, ToughnessLevel.Level3, new DialogueData[2] {
									new DialogueData("How would you even know that I won’t enjoy during the game jam?"),
                                    new DialogueData("Could you not assume things?")
								})
							})
						}),
					# endregion Dialogue 5

					# region Dialogue 6
						new NPCDialogue(new DialogueData[1] {
                            new DialogueData("Sigh, I might have to buy another guitar pick again.", new DialogueButton[3]{
								new DialogueButton("Why? And wait, you play guitar?", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
									new DialogueData("Yeah, I have a band with my friends."),
                                    new DialogueData("And about the guitar pick, I lost it since this morning."),
                                    new DialogueData("I think it fell from my pocket."),
                                    new DialogueData("Oh well, I have to buy another one this month.")
								}),
								new DialogueButton("Why do you even have a guitar pick with you anyways? You didn’t even bring the guitar?", 7, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[4] {
									new DialogueData("Maybe, I just want bring to it anywhere with me?"),
                                    new DialogueData("Oh, you never know, okay?"),
                                    new DialogueData("And it’s not like there’s a rule that whenever you have a guitar pick with you,"),
                                    new DialogueData("you have to bring a guitar along too!")
								}),
                                new DialogueButton("You’re just wasting money. I mean you have your fingers for guitar-playing right?", 7, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[3] {
									new DialogueData("If you’re really into guitars…"),
                                    new DialogueData("You'd notice the huge difference between using fingers and the guitar pick."),
                                    new DialogueData("I bet you didn’t know that, did you?")
								})
							})
						}),
					# endregion Dialogue 6

					# region Dialogue 7
						new NPCDialogue(new DialogueData[3] {
							new DialogueData("I think I'm getting addicted with hot coffee lately"),
                            new DialogueData("I know it's bad but, I can't help it. I needed it to stay awake."),
                            new DialogueData("And now that I'm joining game jam, it will just get worse.", new DialogueButton[3] {
								new DialogueButton("It IS still your choice you know? I mean you could drink another type of beverage to keep you awake.", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3] {
									new DialogueData("True."),
                                    new DialogueData("OR I could've used snacks instead of coffee to keep me awake."),
                                    new DialogueData("Why didn't I think of that before I got a bit addicted?")
								}),
								new DialogueButton("And that's why I never drink coffee!", 8, ButtonType.Wrong, ToughnessLevel.Level3, new DialogueData[1] {
									new DialogueData("Your point what exactly...?")
								}),
                                new DialogueButton("It won't get worse if you CHOSE not to drink it on game jam.", 8, ButtonType.Wrong, ToughnessLevel.Level3, new DialogueData[2] {
									new DialogueData("But, it's really good and it really helps me stay awake!"),
                                    new DialogueData("And there's no other drink that could help me stay awake!")
								})
							})
						}),
					# endregion Dialogue 7

					# region Dialogue 8
						new NPCDialogue(new DialogueData[1] {
							new DialogueData("I'm hungry. I wonder what should I eat?", new DialogueButton[3] {
								new DialogueButton("Just get whatever you feel like eating? I mean I don't know your palate so...", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[6]{
                                    new DialogueData("Then, let's get burgers!"),
                                    new DialogueData("Which fastfood chain though..."),
                                    new DialogueData("It's really hard to pick what food to eat."),
                                    new DialogueData("But, first!"),
                                    new DialogueData("Let me check my wallet hahaha!"),
                                    new DialogueData("Oh. I think... I'll just go for siomai rice. Sigh. Hahaha!")
                                }),
								new DialogueButton("Pasta?", 9, ButtonType.Wrong, ToughnessLevel.Level4, new DialogueData[2]{
                                    new DialogueData("Not really feeling the pasta right now..."),
                                    new DialogueData("Go eat some if you want to though!")
                                }),
                                new DialogueButton("How about noodles?", 9, ButtonType.Wrong, ToughnessLevel.Level4, new DialogueData[2]{
                                    new DialogueData("It's pretty hot outside and you want noodles?"),
                                    new DialogueData("Well, it's not that bad of a choice, but, it is STILL noodles.")
                                })
							})
						}),
					# endregion Dialogue 8

					# region Dialogue 9
						new NPCDialogue(new DialogueData[2] {
							new DialogueData("Do you want to find out my role in the game jam and projects?"),
                            new DialogueData("You'll be surprised!", new DialogueButton[3] {
								new DialogueButton("Sure, go ahead and surprise me. Hahaha!", 5, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3]{
                                    new DialogueData("I'm a sound engineer!"),
                                    new DialogueData("Well, part-time 3D animator, too"),
                                    new DialogueData("But yeah, I do sounds! Were you surprised? hahaha!")
                                }),
								new DialogueButton("I don't think I'll be surprised at all.", 10, ButtonType.Wrong, ToughnessLevel.Level5, new DialogueData[2]{
                                    new DialogueData("Oh c'mon! Can't you even pretend to be surprised if ever?"),
                                    new DialogueData("Ugh, you're so frustrating. Now, I don't feel like telling you anymore.")
                                }),
                                new DialogueButton("I'm not really that interested in knowing. I guess.", 10, ButtonType.Wrong, ToughnessLevel.Level5, new DialogueData[2]{
                                    new DialogueData("Meh, suit yourself."),
                                    new DialogueData("Such a killjoy.")
                                })
							})
						}),
					# endregion Dialogue 9

					# region Dialogue 10

                        // level6 mini game pls
						new NPCDialogue(new DialogueData[4] {
							new DialogueData("I guess I can say I'm ready."),
                            new DialogueData("How about you? Are you ready?"),
                            new DialogueData("'Cause I'm ready! So..."),
                            new DialogueData("Are you really ready?", new DialogueButton[3] {
								new DialogueButton("I'm ready. I'm ready. I'M READY!", ButtonType.Correct, new Statistics(10, 0), new DialogueData[3]{
                                    new DialogueData("That's the spirit! Hahaha!"),
                                    new DialogueData("Good luck staying up in the upcoming 3 days and 2 nights."),
                                    new DialogueData("And good luck for my laptop for the next 3 days and 2 nights.")
                                }),
                                new DialogueButton("I am! Are you ready? 'Cause I'm ready. You should be ready!", ButtonType.Correct, new Statistics(10, 0), new DialogueData[3]{
                                    new DialogueData("Hahaha I wonder how much ready we've said in this conversation."),
                                    new DialogueData("But seriously, I am ready. I just hope my teammates-to-be won't decide on 3D though..."),
                                    new DialogueData("I really don't know how much my laptop could take anymore.")
                                }),
								new DialogueButton("Do you have to ask the same question over and over again?!", ButtonType.Wrong, ToughnessLevel.Level5, new DialogueData[2]{
                                    new DialogueData("Hahaha! Why so mad?"),
                                    new DialogueData("I was just kidding, seriously.")
                                })
							})
						}),
					# endregion Dialogue 10

					},
					new SympathyText(
						new string[1] { "Sorry" }
					),
					new AcceptedText[1] {
						new AcceptedText(
						new DialogueData[1] { 
							new DialogueData("Thank you for giving me ") 
						})
					},
					new DeclinedText[1] {
						new DeclinedText(
						new DialogueData[1] { 
							new DialogueData("Sorry I don't need this") 
						})
					});
                # endregion Bart

                # region Maxine
                npcDataInfoList[7] = new NPCData(
                    "Maxine Paige",
                    TextureResource.AvatarDatabase.PlaceholderAvatar,
                    NPCNameID.Maxine,
                    "A very extroverted artist, who is already working in the industry.",
                    new Statistics(98, 95, 51, 62),
                    new ItemsNeeded[2] {
						new ItemsNeeded(ItemNameID.Headset), new ItemsNeeded(ItemNameID.Milk)
					},
                    new NPCDialogue[10] {

					# region Dialogue 1
						new NPCDialogue(
							new DialogueData[2] {
								new DialogueData("Hi there! Ahahahahaha! You’re one of the lower batches right? Ahahahaha!"),
                                new DialogueData("I’m Maxine, don’t forget okay?", new DialogueButton[2] {
									new DialogueButton("You’re one odd and crazy character.", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[2] {
										new DialogueData("I’ve been told! Hahahaha!"),
                                        new DialogueData("And I’m proud to be hahaha!")
									}),
									new DialogueButton("Okay?", 6, ButtonType.Wrong, ToughnessLevel.Level1, new DialogueData[3] {
										new DialogueData("Geez, don’t be awkward!"),
                                        new DialogueData("I’m your senpai! Senpai!"),
                                        new DialogueData("Lighten up, alright?")
									})
								})
							}
						),
					# endregion Dialogue 1

					# region Dialogue 2
						new NPCDialogue(
							new DialogueData[3] {
                                new DialogueData("We meet again!"),
                                new DialogueData("Is it destiny? Hahaha just kidding!"),
                                new DialogueData("What are you doing wandering in the hallways anyway? Don’t you have class?" , new DialogueButton[2] {
									new DialogueButton("I don't have any, I really am just wandering around!", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[2] {
										new DialogueData("Aren't you bored?"),
                                        new DialogueData("You amaze me at the same time weird me out BUT in a good way! Ahahahaha!")
									}),
									new DialogueButton("Nothing in particular!", 7, ButtonType.Wrong, ToughnessLevel.Level1, new DialogueData[3] {
										new DialogueData("Oooooh~ So defensive!"),
                                        new DialogueData("You're hiding something, aren't you? Aren't you? Ahahaha!"),
                                        new DialogueData("I'll back off now! You touchy touchy, defensive person you! Ahahaha!")
									})
								})
							}
						),
					# endregion Dialogue 2

					# region Dialogue 3
						new NPCDialogue(
							new DialogueData[5] {
								new DialogueData("Hey! If you have troubles with 3D, don’t hesitate to ask me!"),
                                new DialogueData("Uhh contacts, contacts…"),
                                new DialogueData("Ah! Just contact me through facebook"),
                                new DialogueData("or maybe if you know some of my friends like Jenevieve, my love and Franz!"),
                                new DialogueData("Then you can contact me through them! Ahahaha!", new DialogueButton[2] {
									new DialogueButton("Oh, thank you!", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
										new DialogueData("Not a problem, my dear! Ahahaha just kidding!"),
                                        new DialogueData("Actually, it's also one of my excuses so that I could be pm'ed by my real dear and love~"),
                                        new DialogueData("Jenevieeeeveee~ hahahaha!"),
                                        new DialogueData("So contact me as often as possible through he, okay? ahahaha!")
									}),
									new DialogueButton("I don't even know any of them. They sound familiar but, no.", 8, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[4] {
										new DialogueData("Why don't you know them?"),
                                        new DialogueData("You've been studying here for how many years already"),
                                        new DialogueData("and you don't know them?"),
                                        new DialogueData("Well, that sucks. Just contact me in facebook then.")
									})
								})

						}),
					# endregion Dialogue 3

					# region Dialogue 4
						new NPCDialogue(new DialogueData[3] {
                            new DialogueData("I’m so getting in trouble!"),
                            new DialogueData("Damn, I need to find that headset!"),
                            new DialogueData("Where did I put it? Did I lend it to someone?", new DialogueButton[3] {
								new DialogueButton("Maybe you lend it to someone or I don't know. Just remember where you were last in with it?", 5, ButtonType.Correct, new DialogueData[3] {
									new DialogueData("Ah! I'll look for Jenevieve and ask her~! Hahaha!"),
                                    new DialogueData("Another excuse to go back where she is ahaha"),
                                    new DialogueData("Wait. I really should be more worried about the headset.")
								}),
                                new DialogueButton("You could always ask the lost and found you know?", 5, ButtonType.Correct, new Statistics(10, 0), new DialogueData[2] {
									new DialogueData("Oh yeah! It might be there! Hahaha"),
                                    new DialogueData("Thanks for the idea! I hope it's there.")
								}),
								new DialogueButton("You could always buy a new one.", 9, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[2] {
									new DialogueData("I'm already working! I don't want to waste anymore money!"),
                                    new DialogueData("Ugh, I'm deaaaaad.")
								})
							})
						}),
					# endregion Dialogue 4

					# region Dialogue 5
						new NPCDialogue(new DialogueData[5] {
                            new DialogueData("Hey! You joining the game jam?"),
                            new DialogueData("Join the game jam!"),
                            new DialogueData("It’s going to be fun! There’s food! Ahahaha"),
                            new DialogueData("I know I’m joining, so you should too!"),
							new DialogueData("Come on!" , new DialogueButton[3] {
								new DialogueButton("I already decided you before you even asked! Hahaha", 10, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3] {
									new DialogueData("Good!"),
                                    new DialogueData("Then, I don't have to convince you some more."),
                                    new DialogueData("Just be sure to prepare! Hahaha I'll see you there!")
								}),
                                new DialogueButton("Are you really this pushy and convincing?", 10, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
									new DialogueData("Only to people that are not going! Hahaha!"),
                                    new DialogueData("Is it working? If it does, I'll do this to everyone I see who has the look"),
                                    new DialogueData("of not wanting to participate. Hahaha!"),
                                    new DialogueData("I bet I can convince every people I see to join.")
								}),
								new DialogueButton("Are you bribing me to come because of food? Really?", 10, ButtonType.Wrong, ToughnessLevel.Level3, new DialogueData[2] {
									new DialogueData("How would you even know that I won’t enjoy during the game jam?"),
                                    new DialogueData("Could you not assume things?")
								})
							})
						}),
					# endregion Dialogue 5

					# region Dialogue 6
						new NPCDialogue(new DialogueData[3] {
                            new DialogueData("Since I'm already in the industry, don't you want to ask something about it? Hahaha"),
                            new DialogueData("I mean I've been talking to you for how many times now, and you still haven't asked me anything about it"),
                            new DialogueData("which is very surprising. Hahaha!", new DialogueButton[3]{
								new DialogueButton("Ah! I actually wanted to ask before, but, I keep forgetting.", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[1] {
									new DialogueData("Now that I'm here again, ask away! hahaha")
								}),
								new DialogueButton("I just don't know what to ask...? Uh, is the industry out there difficult?", 7, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[5] {
									new DialogueData("I guess it depends on what you think is difficult."),
                                    new DialogueData("In my case, where I work, it's easy to get in and easy to get out."),
                                    new DialogueData("But the real challenge is the monthly test that we have to go through where we should try to pass."),
                                    new DialogueData("There are makeup tests, but, of course, you'd want to have an excellent record."),
                                    new DialogueData("to keep your job.")
								}),
                                new DialogueButton("I'm not confident to ask and know. I might get discouraged.", 7, ButtonType.Wrong, ToughnessLevel.Level2, new DialogueData[4] {
									new DialogueData("Don't be! Hahaha"),
                                    new DialogueData("I'm sure you can take on any challenges!"),
                                    new DialogueData("It's only difficult at the beginning because, you have to adapt to a new set of culture and the likes."),
                                    new DialogueData("But, once you get to used to it, it's just chill! .. well somehow hahaha!")
								})
							})
						}),
					# endregion Dialogue 6

					# region Dialogue 7
						new NPCDialogue(new DialogueData[2] {
							new DialogueData("I wonder what kind of people will be at the game jam this time!"),
                            new DialogueData("Buuuut, I can pretty much guess it's the same old people. Hahaha!", new DialogueButton[3] {
								new DialogueButton("Hey, you never know some new people will participate.", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3] {
									new DialogueData("Expect the unexpected? Hahaha"),
                                    new DialogueData("Quite so, hmm~ it's going to be more interesting then if that's the case!"),
                                    new DialogueData("Can't wait! Hahaha")
								}),
								new DialogueButton("Whoever they are, I'm gonna beat them hands down.", 8, ButtonType.Wrong, ToughnessLevel.Level3, new DialogueData[4] {
									new DialogueData("That's some confidence from you."),
                                    new DialogueData("Just remember to not let all that confidence get in your head! Hahaha"),
                                    new DialogueData("It might get bigger than it already is!"),
                                    new DialogueData("Seriously though, be careful on what you say, okay?")
								}),
                                new DialogueButton("I don't really care that much since the important thing here is to learn from this.", 8, ButtonType.Wrong, ToughnessLevel.Level3, new DialogueData[2] {
									new DialogueData("How boring!"),
                                    new DialogueData("Where's your competitive spirit?!")
								})
							})
						}),
					# endregion Dialogue 7

					# region Dialogue 8
						new NPCDialogue(new DialogueData[2] {
							new DialogueData("I'm still thinking about whether to buy a drink or not."),
                            new DialogueData("I'd like some milk but, I'll eat soon anyways.", new DialogueButton[3] {
								new DialogueButton("Hmm, why don't you buy one and when you eat with everyone else," + 
                                                    "don't order a drink anymore and just drink the milk you bought.", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[2]{
                                    new DialogueData("I can do that but I want something frizzy to drink, too! Hahaha!"),
                                    new DialogueData("I just can't decide!")
                                }),
								new DialogueButton("Just buy one and get over it. You'll buy one in the end anyways.", 9, ButtonType.Wrong, ToughnessLevel.Level4, new DialogueData[3]{
                                    new DialogueData("Are you in a hurry or something?"),
                                    new DialogueData("If you want, you can go ahead!"),
                                    new DialogueData("You don't have to wait for me to get to talk to me, you might see me later anyways!")
                                }),
                                new DialogueButton("I'll eat ahead of you with the others. See you", 9, ButtonType.Wrong, ToughnessLevel.Level4, new DialogueData[3]{
                                    new DialogueData("Ooohh, so that's the way you want it"),
                                    new DialogueData("Well..."),
                                    new DialogueData("I don't mind at all! Hahaha")
                                })
							})
						}),
					# endregion Dialogue 8

					# region Dialogue 9
						new NPCDialogue(new DialogueData[7] {
							new DialogueData("Have you decided what type of game you might want to make on game jam?"),
                            new DialogueData("Pshh, theme doesn’t matter as long as you can incorporate your idea on it."),
                            new DialogueData("So… are you going 2D or 3D?"),
                            new DialogueData("It’s much more advisable to do 2D if it’s 3 days but, if you have an excellent 3D artist"),
                            new DialogueData("or if you’re an excellent 3D artist yourself..."),
                            new DialogueData("then, what the heck? Go for 3D!"),
                            new DialogueData("Or you might want 2.5D? Hahaha!", new DialogueButton[3] {
								new DialogueButton("I’m not exactly sure yet. I’ll consult with my teammates-to-be during game jam!", 5, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4]{
                                    new DialogueData("Suit yourself!"),
                                    new DialogueData("Just be ready during on the brainstorming event alright? Ahaha"),
                                    new DialogueData("After all, everyone has to pitch in their game ideas during that time!"),
                                    new DialogueData("I’m sure everyone will do fine!")
                                }),
								new DialogueButton("I want to do 3D! 3D's easy peasy~", 10, ButtonType.Wrong, ToughnessLevel.Level5, new DialogueData[5]{
                                    new DialogueData("If you decide on that, just make sure you finish the game okay?"),
                                    new DialogueData("You can still rethink this though while you still have the time."),
                                    new DialogueData("Good luck! Hahaha"),
                                    new DialogueData("I mean if 3D's easy for you then I'll look forward to the game that you'll create with it!"),
                                    new DialogueData("If you're having a hard time, not my fault you thought 3D was easy~ hahaha!")
                                }),
                                new DialogueButton("Maybe a 2D or 2.5D!", 10, ButtonType.Wrong, ToughnessLevel.Level5, new DialogueData[3]{
                                    new DialogueData("Geez, decide only one! Hahaha!"),
                                    new DialogueData("You can't be indecisive during game jam okay?"),
                                    new DialogueData("Every second counts!")
                                })
							})
						}),
					# endregion Dialogue 9

					# region Dialogue 10

                        // level6 mini game pls
						new NPCDialogue(new DialogueData[3] {
							new DialogueData("Are you ready?"),
                            new DialogueData("Come on! Ahaha Stop being nervous!"),
                            new DialogueData("Game jam is there to let you enjoy creating games and getting along with other people!", new DialogueButton[3] {
								new DialogueButton("Alright! I'll do my best to not get nervous. Hahaha", ButtonType.Correct, new Statistics(10, 0), new DialogueData[3]{
                                    new DialogueData("I can see the nervousness though hahaha!"),
                                    new DialogueData("But I guess that's better than pure nervousness though! Since..."),
                                    new DialogueData("I can also see you're as excited as me! Hahaha!")
                                }),
                                new DialogueButton("I know that and I AM controlling my nerves.", ButtonType.Correct, new Statistics(10, 0), new DialogueData[5]{
                                    new DialogueData("Well, control them more! Hahaha"),
                                    new DialogueData("Until all I see is excitement and confidence!"),
                                    new DialogueData("If you have something to be proud off that would help you in game jam,"),
                                    new DialogueData("Flaunt it or at least show them that you're proud alright? hahaha"),
                                    new DialogueData("If you can do that, then you're ready!")
                                }),
								new DialogueButton("I can't help it okay?!", ButtonType.Wrong, ToughnessLevel.Level5, new DialogueData[2]{
                                    new DialogueData("Hey hey~ Chill!"),
                                    new DialogueData("Just encouraging you to not let nervousness get to you.")
                                })
							})
						}),
					# endregion Dialogue 10

					},
					new SympathyText(
						new string[1] { "Sorry" }
					),
						new AcceptedText[1] {
						new AcceptedText(
						new DialogueData[1] { 
							new DialogueData("Thank you for giving me ") 
						})
					},
					new DeclinedText[1] {
						new DeclinedText(
						new DialogueData[1] { 
							new DialogueData("Sorry I don't need this") 
						})
					});
                # endregion Maxine

				// Used for shuffling dialogue lines
				sortingDataList = new List<NPCData>(npcDataInfoList);
			}

			// --- DO NOT EDIT BELOW THIS LINE --- //

			public static void AddNPCStats(NPCNameID nameID, Statistics addedStat) {
				if (nameID != NPCNameID.None) {
					for (int i = 0; i < npcDataInfoList.Length; i++) {
						if (npcDataInfoList[i].NpcNameID == nameID) {
							NPCData tmpData = npcDataInfoList[i];
							Statistics beforeStat = tmpData.NpcStatistics;
							Statistics afterStat = tmpData.NpcStatistics + addedStat;
							if (addedStat.Like > 0) {
								afterStat.Dislike -= addedStat.Like;
							}
							else if (addedStat.Dislike > 0) {
								afterStat.Like -= addedStat.Dislike;
							}
							afterStat.ClampAllValues();
							tmpData.NpcStatistics = afterStat;
							tmpData.NpcStatistics.ClampAllValues();

							// Debug Result
							Debug.Log("[NPC STATISTICS] " + addedStat.ToString() + " has been added to " + 
								tmpData.NpcName + " " + beforeStat.ToString() + " -> " + afterStat.ToString());
							break;
						}
					}
				}
			}

			public static NPCData GetNPC(NPCNameID nameID) {
				NPCData npcData = new NPCData();
				if (nameID != NPCNameID.None) {
					for (int i = 0; i < npcDataInfoList.Length; i++) {
						if (npcDataInfoList[i] != null && npcDataInfoList[i].NpcNameID == nameID) {
							npcData = npcDataInfoList[i];
						}
					}
				}
				else {
					Debug.Log("NameID is None. NPCData -> GetNPC");
				}
				return npcData;
			}

			public static ItemsNeeded GetNpcItemNeeded(NPCNameID nameID, ItemNameID itemID) {
				NPCData tmpNpcData = GetNPC(nameID);
				ItemsNeeded itemNeeded = new ItemsNeeded();
				for (int i = 0; i < tmpNpcData.NpcItemsNeeded.Length; i++) {
					if (tmpNpcData.NpcItemsNeeded[i].ItemID == itemID) {
						itemNeeded = tmpNpcData.NpcItemsNeeded[i];
					}
				}
				return itemNeeded;
			}

			public static void GiveItemToNpc(NPCNameID nameID, ItemNameID itemID, int inventoryIndx) {
				NPCManager npcManager = NPCManager.current;
				UserInterface userInterface = UserInterface.current;
				GameManager gameManager = GameManager.current;
				PlayerInformation playerInformation = gameManager.BasePlayerData.PlayerInformation;

				ItemsNeeded itemRecieved = GetNpcItemReceived(nameID, itemID);
				NPCInformation npcInformation = npcManager.GetMainNpcInformation(nameID);

				userInterface.GivingItem = false;
				playerInformation.RemoveInventoryItem(itemID, inventoryIndx);

				if (itemRecieved.ItemID != ItemNameID.None && !itemRecieved.ItemRecieved) {
					itemRecieved.ItemRecieved = true;
					npcInformation.AddItemsHave(itemID);			

					if (itemRecieved.AddedStatistics != Statistics.zero) {
						AddNPCStats(nameID, itemRecieved.AddedStatistics);
						Debug.Log("[NPC STATISTICS] " + npcInformation.NpcName + " has Incresed Stats " + itemRecieved.AddedStatistics.ToString());
					}

					npcInformation.RunEmoticon(EmoticonNameID.Happy);
					DialogueManager.current.RunReplyDialogue(nameID, ReplyType.Accept, ItemDatabase.GetItem(itemID).ItemName);
				}
				else {
					ItemData item = ItemDatabase.GetItem(itemID);
					Statistics beforeStat = npcInformation.NpcStatistics;
					Statistics afterStat = beforeStat + item.ItemDebuffStat;
					AddNPCStats(nameID, item.ItemDebuffStat);

					Debug.Log("[NPC ITEM NEEDED] " + npcInformation.NpcName + " doesn't require " + item.ItemName + ".");
					Debug.Log("[NPC ITEM DEBUFF] " + npcInformation.NpcName + " has recieved a debuff of "
						+ item.ItemDebuffStat + " because of wrong item. " + beforeStat.ToString() + " -> " + afterStat.ToString());

					npcInformation.RunEmoticon(EmoticonNameID.Sad);
					DialogueManager.current.RunReplyDialogue(nameID, ReplyType.Decline, ItemDatabase.GetItem(itemID).ItemName);
				}
			}

			public static ItemsNeeded GetNpcItemReceived(NPCNameID nameID, ItemNameID itemID) {
				NPCData tmpNpcData = GetNPC(nameID);
				ItemsNeeded itemNeeded = new ItemsNeeded();
				for (int i = 0; i < tmpNpcData.NpcItemsNeeded.Length; i++) {
					if (tmpNpcData.NpcItemsNeeded[i].ItemID == itemID && !tmpNpcData.NpcItemsNeeded[i].ItemRecieved) {
						itemNeeded = tmpNpcData.NpcItemsNeeded[i];
						break;
					}
				}
				return itemNeeded;
			}

			public static NPCDialogue GetNPCDialogue(int indx, NPCNameID nameID) {
				NPCDialogue npcDialogue = new NPCDialogue();
				if (nameID != NPCNameID.None) {
					npcDialogue = GetNPCDialogueList(nameID)[indx];
				}
				else {
					Debug.Log("NameID is None. NPCData -> GetNPCDialogue");
				}
				return npcDialogue;
			}

			public static NPCDialogue[] GetNPCDialogueList(NPCNameID nameID) {
				if (nameID != NPCNameID.None) {
					return GetNPC(nameID).NpcDialogue;
				}
				Debug.Log("NameID is None. NPCData -> GetNPCDialogueList");
				return null;
			}

			public static DialogueData[] GetNPCSelectionDialogueData(int indx, NPCNameID nameID) {
				if (nameID != NPCNameID.None) {
					return GetNPCDialogue(indx, nameID).DialogueData;
				}
				Debug.Log("NameID is None. NPCData -> GetNPCSelection");
				return null;
			}

			public static DialogueData[] GetNPCContinousDialogueData(NPCNameID nameID) {
				if (nameID != NPCNameID.None) {
					NPCData tmpSortingData = GetNPC(nameID);
					NPCDialogue[] npcDialogue = tmpSortingData.NpcDialogue;
					DialogueData[] result = new DialogueData[npcDialogue.Length];
					int indx = 0;

					if (npcDialogue != null) {
						indx = tmpSortingData.NpcDialogueIndx;
						indx++;
						tmpSortingData.NpcDialogue = npcDialogue;
						if (indx > tmpSortingData.NpcDialogue.Length - 1) {
							indx = 0;
						}

						tmpSortingData.NpcDialogueIndx = indx;
						result = tmpSortingData.NpcDialogue[indx].DialogueData;
						return result;
					}
					else
						return null;
				}
				Debug.Log("NameID is None. NPCData -> GetNPCScriptDialogue");
				return null;
			}

			public static DialogueData[] GetNPCRandomDialogueData(NPCNameID nameID) {
				if (nameID != NPCNameID.None) {
					NPCData tmpSortingData = sortingDataList[(int)nameID - 1];
					if (tmpSortingData.NpcDialogue != null) {
						List<NPCDialogue> scriptDialogueList = new List<NPCDialogue>(tmpSortingData.NpcDialogue);
						DialogueData[] result = new DialogueData[scriptDialogueList.Count];
						result = scriptDialogueList[0].DialogueData;

						// Iterate through the list and keep the list shuffling.
						// This prevents the result from being repeated twice
						if (scriptDialogueList.Count > 1) {
							do {
								scriptDialogueList.Sort((x, y) => Random.value < 0.5f ? -1 : 1);
							}
							while (result == scriptDialogueList[0].DialogueData);
						}

						tmpSortingData.NpcDialogue = scriptDialogueList.ToArray();
						result = scriptDialogueList[0].DialogueData;
						return result;
					}
					else
						return null;
				}
				Debug.Log("NameID is None. NPCData -> GetNPCRandomDialogueList");
				return null;
			}

			public static DialogueData[] GetNPCRandomAcceptedText(NPCNameID nameID) {
				if (nameID != NPCNameID.None) {
					NPCData tmpSortingData = sortingDataList[(int)nameID - 1];
					if (tmpSortingData.NpcAcceptedText != null) {
						List<AcceptedText> acceptedTextList = new List<AcceptedText>(tmpSortingData.NpcAcceptedText);
						DialogueData[] result = new DialogueData[acceptedTextList.Count];
						result = acceptedTextList[0].DialogueData;

						if (acceptedTextList.Count > 1) {
							do {
								acceptedTextList.Sort((x, y) => Random.value < 0.5f ? -1 : 1);
							}
							while (result == acceptedTextList[0].DialogueData);
						}

						tmpSortingData.NpcAcceptedText = acceptedTextList.ToArray();
						result = acceptedTextList[0].DialogueData;
						return result;
					}
					else 
						return null;
				}
				Debug.Log("NameID is None. NPCData -> GetNPCRandomAcceptedText");
				return null;
			}

			public static DialogueData[] GetNPCRandomDeclineText(NPCNameID nameID) {
				if (nameID != NPCNameID.None) {
					NPCData tmpSortingData = sortingDataList[(int)nameID - 1];
					if (tmpSortingData.NpcDeclinedText != null) {
						List<DeclinedText> declinedTextList = new List<DeclinedText>(tmpSortingData.NpcDeclinedText);
						DialogueData[] result = new DialogueData[declinedTextList.Count];
						result = declinedTextList[0].DialogueData;

						if (declinedTextList.Count > 1) {
							do {
								declinedTextList.Sort((x, y) => Random.value < 0.5f ? -1 : 1);
							}
							while (result == declinedTextList[0].DialogueData);
						}

						tmpSortingData.NpcDeclinedText = declinedTextList.ToArray();
						result = declinedTextList[0].DialogueData;
						return result;
					}
					else
						return null;
				}
				Debug.Log("NameID is None. NPCData -> GetNPCRandomAcceptedText");
				return null;
			}
		}
	}
}