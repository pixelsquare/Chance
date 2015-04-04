using System.Collections.Generic;
using Item;
using Item.Database;
using UnityEngine;
using MiniGames;

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

			private static List<NPCData> sortingDataList = new List<NPCData>();

			public static void Initialize() {
				# region Test Nina
				npcDataInfoList[0] = new NPCData(
					"Nina",
					"Nina",
					Resources.AvatarDatabase.NinaAvatar,
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
					new ItemAcceptDialogue[1] {
						new ItemAcceptDialogue(
						new DialogueData[1] { 
							new DialogueData("Thank you for giving me ") 
						})
					},
					new ItemDeclineDialogue[1] {
						new ItemDeclineDialogue(
						new DialogueData[1] { 
							new DialogueData("Sorry I don't need this") 
						})
					});
				# endregion Test Nina

				# region Test Beta
				npcDataInfoList[1] = new NPCData(
					"Beta",
					"Beta",
					Resources.AvatarDatabase.BetaAvatar,
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
								new DialogueData(Resources.AvatarDatabase.NinaAvatar,"Do you really think you can outsmart a robot?"),
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
					new ItemAcceptDialogue[1] {
						new ItemAcceptDialogue(
						new DialogueData[1] { 
							new DialogueData("Thank you for giving me ") 
						})
					},
					new ItemDeclineDialogue[1] {
						new ItemDeclineDialogue(
						new DialogueData[1] { 
							new DialogueData("Sorry I don't need this") 
						})
					});

				# endregion Test Beta

                # region Noelle
                npcDataInfoList[2] = new NPCData(
					"Noelle", 
                    "Noelle Ingram",
                    Resources.AvatarDatabase.NoelleAvatar,
                    NPCNameID.Noelle,
                    "An indecisive designer, who had just established her future goals. She's crazy over shakes and secretly crazy over writing fanfictions.",
                    new Statistics(83, 93, 44, 56),
                    new ItemsNeeded[2] {
						new ItemsNeeded(ItemNameID.FanfictionNotebook), new ItemsNeeded(ItemNameID.StrawberryMilkshake)
					},
                    new NPCDialogue[10] {

					# region Dialogue 1
						new NPCDialogue(
							new DialogueData[1] {
								new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Uh, hi.", new DialogueButton[2] {
									new DialogueButton(Resources.AvatarDatabase.NoelleHappyAvatar, "Hi there! Classmates last Tuesday right?", 1, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
										new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Yeah. And yesterday, too, but…"),
                                        new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "As far as I could remember from the surnames being called for attendance..."),
                                        new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "You weren’t there right?"),
										new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Ah I didn’t mean to snoop around or anything, I really just remembered.")
									}),
									new DialogueButton("… Hi.", 5, ButtonType.Wrong, KeypressLevel.Level1, new DialogueData[1] {
										new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "R-right /awkward silence/")
									})
								})
							}
						),
					# endregion Dialogue 1

					# region Dialogue 2
						new NPCDialogue(
							new DialogueData[6] {
                                new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar, "So, um… wha- ack! /cough cough/"),
                                new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Oh, uh, sorry, that was a bit embarrassing."),
                                new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "I can’t believe I got choked by air."),
                                new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "The nerves got to me. "),
                                new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "It’s just that…this is the first time I actually spoke to you one on one."),
								new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "So,um, what brings you here in school today? Class?", new DialogueButton[2] {	
									new DialogueButton("Uh...Maybe I have or maybe I don't have a class...", 6, ButtonType.Wrong, KeypressLevel.Level1, new DialogueData[4] {
										new DialogueData(Resources.AvatarDatabase.NoelleSadAvatar, "Am I too weird?"),
										new DialogueData(Resources.AvatarDatabase.NoelleSadAvatar, "I only did it by accident."),
                                        new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar, "Well, if you find me too weird..."),
                                        new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar, "Then just, go away..!")
									}),
                                    new DialogueButton("Pfft. Ahahahaha! Ehem! Sorry about that.", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3] {
										new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Um, no. It’s okay. I’m kind of used to those kinds of reaction."),
										new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Aaaaand I’m weird like that. I guess! Haha"),
										new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "But, hey, the tension’s gone at least, right?")
									})
								})
							}
						),
					# endregion Dialogue 2

					# region Dialogue 3
						new NPCDialogue(
							new DialogueData[3] {
								new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "You know…you haven’t really answered my question before."),
                                new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "But, nevermind that!"),
								new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Let’s change topic… if that is you don’t mind.", new DialogueButton[2] {
									new DialogueButton("How about interests? Any interests, besides games?", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5] {
										new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "I have a LOT actually. Where to start…"),
										new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "I like Anime, Manga and Kpop…uh…"),
                                        new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "I love food in general AND strawberry milkshakes or banana shakes"),
                                        new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "I love and do writing and reading fanfictions."),
										new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "I think; I should stop it until there. It’s really a long list I tell you!")
									}),
									new DialogueButton("Err, do you have any topic in mind?", 7, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[4] {
										new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar,"Um, I was kinda hoping you’d be the one choosing the topic?"),
                                        new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar, "Y’know, you don’t have to force yourself talking with me."),
                                        new DialogueData(Resources.AvatarDatabase.NoelleSadAvatar, "I-I think! I think I should go. Um, class…"),
										new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "I have class! Yeah, I have class… so yeah.")
									})
								})
                            }
                         ),
					# endregion Dialogue 3

					# region Dialogue 4
						new NPCDialogue(new DialogueData[3] {
                            new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "So… what’s your role?"),
							new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "It’s either you’re an artist, programmer, designer or producer right?"),
                            new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Take your pick~", new DialogueButton[3] {
								new DialogueButton("I can do art~", 4, ButtonType.Correct, new DialogueData[4] {
									new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Oooh~ An artist!"),
									new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Let me see your work~ :3"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleSadAvatar,"Pleeeaaaaassseeee~"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar,"Please Oh please oh please oh please oh please oh please!!!!")
								}),
                                new DialogueButton("Put me in any spot! I can take care of everything!", 8, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[3] {
									new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar, "You're one cocky person, aren't you?"),
									new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar, "If you're that cocky, can I see any of your works?"),
									new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "I'll acknowledge your bragging rights when you have something to show me.")
								}),
                                new DialogueButton("I can program decently, I suppose.", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
									new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Do you have sample games I can try~?"),
									new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "What languages are you good at~?"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "I'll add you to my imaginary list of programmers in our batch~!"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Hehe~ I do hope you participate in the incoming event")
								})		
							})
						}),
					# endregion Dialogue 4

					# region Dialogue 5
						new NPCDialogue(new DialogueData[1] {
							new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar,"It’s that time again.", new DialogueButton[3] {
								new DialogueButton("Yeah, it's game jam again!", 9, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5] {
									new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "That’s right. Cool, you’re participating, too!"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Best of luck to the both of us!"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Let’s conquer the three days and two nights of no sleep!"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Not that I actually slept in the previous game jams..."),
                                     new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Since I did sleep unlike my teammate. Hahaha!")
								}),
                                new DialogueButton("Game Jam, right...?", 9, ButtonType.Correct, new Statistics(10, 0), new DialogueData[8] {
									new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "What's wrong? Not exciting enough?"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Or maybe... discouraged?"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Sigh, that's nonsense! Just give it your all!"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "We can do this!"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "I mean it's not everyday you can do something like that right?"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "It's a once a year thing."),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "While we have the time because we're students..."),
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "We should take advantage of it and enjoy, okay?")
								}),
								new DialogueButton("Time for what?", 9, ButtonType.Wrong, KeypressLevel.Level3, new DialogueData[5] {
									new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Oh, you know. Game jam?"),	
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Erm, how do you not know that it’s game jam season?"),	
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "You didn't participate the previous years?"),
	                                new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar, "HOW COULD YOU NOT PARTICIPATE?"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Ehem...it WAS a REQUIREMENT in our year.")
								})
							})
						}),
					# endregion Dialogue 5

					# region Dialogue 6
						new NPCDialogue(new DialogueData[2] {
                            new DialogueData(Resources.AvatarDatabase.NoelleSadAvatar, "Sigh, it’s so stressful."),
							new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Ah-! You were here? Sorry, I guess I was too deep in thought.", new DialogueButton[3]{
                                new DialogueButton("What’s stressful?", 6, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[2] {
									new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar, "Ugh, please."),
									new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar, "Mind your own business.")	
								}),
								new DialogueButton("You should be careful next time." + "You might bump into something.", 1, ButtonType.Correct, new Statistics(10, 0), new DialogueData[6] {
									new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Oh, thanks."),
									new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "I’m pretty much a klutz so..."),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "It’s pretty normal if you see me bump into something or someone."),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Most of the time, the reason why I bump into something is that..."),
									new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "...I can only keep and focus my attention on something one at a time. Hahaha!"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "But...I’m doing my best multitasking...erm, I suppose...")
								}),	
                                new DialogueButton("You know. It would slightly help" + "if you vent out those stress.", 6, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[1] {
									new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar, "I know.")
								})
							})
						}),
					# endregion Dialogue 6

					# region Dialogue 7
						new NPCDialogue(new DialogueData[3] {
							new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar, "Just great…JUST GREAT. I have to go and do this and this…"),
                            new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar, "Tsk."),
                            new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Would you stop following me?", new DialogueButton[3] {
								
								new DialogueButton("I'm not even following you...", 7, ButtonType.Wrong, KeypressLevel.Level3, new DialogueData[3] {
									new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Nevermind."),
	                                new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Just go away, okay?"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar, "Clearly, I'm not in the mood.")
								}),
                                new DialogueButton("Why are you so irritated?" + "Monthly period or something?", 7, ButtonType.Wrong, KeypressLevel.Level3, new DialogueData[2] {
									new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar, "That's just!"),
	                                new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar, "Ugh! You're asking for it!")
								}),
                                new DialogueButton("What the hell’s your problem?", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[6] {
									new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Oh, um… sorry."),
	                                new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "I was just overwhelmed by the added things I have to do"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "When I’m not even finished with others."),
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "And I..I’m hungry. I didn’t get to eat any for the day yet hehe.."),
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "I get cranky sometimes when I’m hungry..but that’s not excuse, so yeah"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleSadAvatar, "Still...I apologize for that episode.")
								})
							})
						}),
					# endregion Dialogue 7

					# region Dialogue 8
						new NPCDialogue(new DialogueData[5] {
							new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Yo! I've been seeing you more and more in the hallway lately!"),
                            new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Not that it's bad or anything."),
                            new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Um, let's see here. I actually wanted to ask you something"),
                            new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Have you seen some kind of notebook around?"),
                            new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "A-Ah, not that there's something special in it or anything at all!", new DialogueButton[3] {
								new DialogueButton("I'm not sure if I've seen one." + "If ever I come across it, I'll give it to you.", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3]{
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Oh~ Thank you thank you thank you thank you!"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "DON'T read it okay?"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar, "It's NOT interesting so just give it to me IMMEDIATELY.")
                                }),
								new DialogueButton("Now, I'm quite interested on learning what's inside" + "once I find it first.", 8, ButtonType.Wrong, KeypressLevel.Level4, new DialogueData[3]{
                                    new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar, "Don't you dare!"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar, "Seriously, don't you DARE!"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar, "I WILL find out if you read it or even dared open it")
                                }),
                                new DialogueButton("I do wonder. Did I see one or not?" + "I might as well check it out myself.", 8, ButtonType.Wrong, KeypressLevel.Level4, new DialogueData[3]{
                                    new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar, "Nonononononononono!"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleSadAvatar, "Please I beg you! If it's with you, please don't!"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleSadAvatar, "T^T NOOOO~")
                                })
							})
						}),
					# endregion Dialogue 8

					# region Dialogue 9
						new NPCDialogue(new DialogueData[1] {
							new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "It's a good thing I have a laptop!", new DialogueButton[3] {
                                new DialogueButton("...And then on the day of the event, it crashes.", 9, ButtonType.Wrong, KeypressLevel.Level5, new DialogueData[2]{
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Don't you dare jinx that."),
                                    new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar, "If that happens, I'll blame you.")
                                }),
								new DialogueButton("Desktops are better than laptops!", 9, ButtonType.Wrong, KeypressLevel.Level5, new DialogueData[3]{
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Well, laptops are more convenient with this type of event!"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Not unless you decide to go home and just come back the next day..."),
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "But still, a hassle!")
                                }),
                                new DialogueButton("Why is that?", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[9]{
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Well, if you’re planning to stay overnight in the school during the game jam"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "...you won’t have to worry and bring your desktop from home"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "...just to set it up in the venue."),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "That would be a hassle, don’t you think? I mean…"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Number 1, they’re heavy."),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Number 2, a lot of wires to setup and so little space in the venue."),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "And Number 3, you’ll bring a lot of the parts like"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "The CPU then the keyboard, then the monitor, etc."),
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Not unless you have a car...")
                                })
                                
							})
						}),
					# endregion Dialogue 9

					# region Dialogue 10

                        // level6 mini game pls
						new NPCDialogue(new DialogueData[1] {
							new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Hmm~ So excited", new DialogueButton[3] {
								new DialogueButton("Haha, you're that excited huh?", ButtonType.Correct, new Statistics(10, 0), new DialogueData[6]{
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Weeeeell~ I have some ideas stacked up and ready for game jam."),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Though, I’ll worry about the theme on the day itself…Still!"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "I’m ready! I have my Unity, RPGMaker, Construct2, Renpy.."),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar,"Erm I don’t have Game Maker though, but, I have Photoshop!"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Then, there’s Maya…uh… how about you?"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Are you prepared?")
                                }),
                                new DialogueButton("I look forward to it." + "Just not as hyped up as you. Haha.", ButtonType.Correct, new Statistics(10, 0), new DialogueData[7]{
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Oh, too hyper for you?"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Sorry about that. I couldn't help it"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "I made preparations for it though!"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Have you packed yet? Or... did you decide not to overnight?"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "Speaking of overnight, I could actually not stay overnight"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleWeirdAvatar, "My dorm IS just a walk away."),
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar,"And it's more fun doing the jam with all your old and new friends.")
                                }),
								new DialogueButton("I'm not excited at all." + "I'm more nervous.", ButtonType.Wrong, KeypressLevel.Level5, new DialogueData[3]{
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "Don't be!"),
                                    new DialogueData(Resources.AvatarDatabase.NoelleHappyAvatar, "I'm letting all this excitement out to remove my anxiety."),
                                    new DialogueData(Resources.AvatarDatabase.NoelleMadAvatar,"If you keep on being nervous over there, you'll affect my excitement!")
                                })
							})
						}),
					# endregion Dialogue 10

					},
					new SympathyText(
						new string[4] { "I'm sorry.", "Forgive me!", "A misunderstanding!", "I didn't mean to." }
					),
					new ItemAcceptDialogue[1] {
						new ItemAcceptDialogue(
						new DialogueData[1] { 
							new DialogueData("How did you know? Thank you for handing me this") 
						})
					},
					new ItemDeclineDialogue[1] {
						new ItemDeclineDialogue(
						new DialogueData[1] { 
							new DialogueData("This is not what I was looking for. Sorry I don't need this") 
						})
					});
                # endregion Noelle

                # region Andy
                npcDataInfoList[3] = new NPCData(
					"Andy",
                    "Andy Gibson",
                    Resources.AvatarDatabase.AndyAvatar,
                    NPCNameID.Andy,
                    "An ambitious programmer, who is very focused in his goals and his future. He also has to have his daily dose of milk tea and backing up of files.",
                    new Statistics(40, 68, 97, 81),
                    new ItemsNeeded[2] {
						new ItemsNeeded(ItemNameID.USB), new ItemsNeeded(ItemNameID.GrassJellyMilkTea)
					},
                    new NPCDialogue[10] {

					# region Dialogue 1
						new NPCDialogue(
							new DialogueData[1] {
								new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "...", new DialogueButton[2] {
									new DialogueButton("Um, is there something wrong with my face?", 1, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3] {
										new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Nothing really."),
                                        new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "Sorry if I managed to stare at you."),
                                        new DialogueData(Resources.AvatarDatabase.AndySadAvatar, "I was just looking around while waiting for somebody")
									}),
									new DialogueButton("You shouldn’t stare at other people like that.", 5, ButtonType.Wrong, KeypressLevel.Level1, new DialogueData[2] {
										new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "I wasn’t staring exactly at you. I was looking around."),
                                        new DialogueData(Resources.AvatarDatabase.AndyMadAvatar, "So assuming...")
									})
								})
							}
						),
					# endregion Dialogue 1

					# region Dialogue 2
						new NPCDialogue(
							new DialogueData[1] {
                                new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "Not to be rude but, you’re quite different from before.", new DialogueButton[2] {
									new DialogueButton("Oh, I didn’t think anyone would even notice" + "someone like me before.", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5] {
										new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "That’s probably just me."),
										new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "I tend to observe around when I’m not interested with the class."),
										new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "And we have the same class last Tuesday."),
                                        new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "You’ll probably see me with a kid-looking girl if you didn’t see me."),
                                        new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "That’s why I recognize you.")
									}),
									new DialogueButton("Dude, that's a bit creepy." + "Not everybody should be that sharp.", 6, ButtonType.Wrong, KeypressLevel.Level1, new DialogueData[4] {
										new DialogueData(Resources.AvatarDatabase.AndySadAvatar, "I'm sorry for noticing too many things then."),
                                        new DialogueData(Resources.AvatarDatabase.AndySadAvatar, "You didn't have to go say I was creepy."),
                                        new DialogueData(Resources.AvatarDatabase.AndyMadAvatar,"Meh, I shouldn't have even bothered."),
                                        new DialogueData(Resources.AvatarDatabase.AndyMadAvatar,"Such a waste of time.")
									})
								})
							}
						),
					# endregion Dialogue 2

					# region Dialogue 3
						new NPCDialogue(
							new DialogueData[1] {
								new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "It's you again. What's up?", new DialogueButton[2] {
                                    new DialogueButton("Uh, the clouds? Erm, the ceiling?", 7, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[8] {
										new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "Okaaaay."),
                                        new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "Don't...try that again."),
                                        new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "One, if you want to joke, use something new."),
                                        new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "Two, if you want to joke, choose the right timing please"),
                                        new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "And three, if you want to joke something like that,"),
                                        new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "...be sure to use only ONE answer."),
                                        new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Don't doubt your punchline."),
                                        new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "I'm no comedian, but, at least I know that wasn't funny.")
									}),
									new DialogueButton("Oh you know..." + "Wondering if I should join the Game Jam...", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3] {
										new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Join it."),
                                        new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "I'm not forcing you to but, I'm recommending you to join it."),
                                        new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Just saying~ hahaha")
									})
									
								})

						}),
					# endregion Dialogue 3

					# region Dialogue 4
						new NPCDialogue(new DialogueData[2] {
                            new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "I'm a bit stuck with something right now."),
                            new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Are you any good with concepts or ideas at least?", new DialogueButton[3] {
								new DialogueButton("I think I'm a bit above average?", 4, ButtonType.Correct, new DialogueData[5] {
									new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "That's great!"),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "I really need some help over creating a game."),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Not that big of a game though."),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Something to be created within a span of one night"),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "I know it sounds ridiculous but, I really need an idea.")
								}),
                                new DialogueButton("What is it for?", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5] {
									new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Oh I just wanted to create a very simple game for myself."),
									new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Just a good practice to keep my programming skills sharp."),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "I don't usually ask for others since I have usually have a friend designer."),
                                    new DialogueData(Resources.AvatarDatabase.AndySadAvatar, "But, well... something happened."),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "I need your help, if you don't mind.")
								}),
								new DialogueButton("Hehe~ Do I get paid?", 8, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[3] {
									new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "I don't mind paying but..."),
                                    new DialogueData(Resources.AvatarDatabase.AndyMadAvatar, "YOU don't get ahead with the client"),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Sigh, I'll look for someone else to help me instead.")
								})
							})
						}),
					# endregion Dialogue 4

					# region Dialogue 5
						new NPCDialogue(new DialogueData[7] {
							new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "Have you ever had this condition where..."),
                            new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "You work better in a specific environment?"),
                            new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "I mean like the place and the time of work?"),
                            new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "Because, I can't help but wonder..."),
                            new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "...if I really should stay overnight in school"),
                            new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "...when I work better when I'm at home during the night."),
                            new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Oh wait, nevermind. I suddenly remembered that one rule." , new DialogueButton[3] {
								new DialogueButton("What one rule?", 9, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3] {
									new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "It's the rule where if one decides not to stay overnight and just go home"),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "That person shouldn't work on the game at home"),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "...and instead come back the next day to work on it.")
								}),
                                new DialogueButton("I never really noticed things like that.", 9, ButtonType.Correct, new Statistics(10, 0), new DialogueData[9] {
									new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Well, after the game jam, you should find out for yourself."),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "It'll help you with scheduling your work and all that."),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Like what I do..."),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Since I don't really work well in the mornings and a bit of the afternoon"),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "I put all my freetime there and just relax or maybe sleep"),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "...and work at night until 5 am"),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "I also don't really work well when I'm in an overnight or"),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "if there's some kind of disturbance around me, because I can't focus."),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "That's why I like working in my room at home.")
								}),
								new DialogueButton("Not really. I don't care as much at all.", 9, ButtonType.Wrong, KeypressLevel.Level3, new DialogueData[3] {
									new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Well, I'll go with your flow then."),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Don't say I didn't try to remind you."),
                                    new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "Your choice, not my problem anymore.")
								})
							})
						}),
					# endregion Dialogue 5

					# region Dialogue 6
						new NPCDialogue(new DialogueData[3] {
                            new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "So thirsty...I could use some iced tea- ah no. milk tea..."),
							new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Oh? You were there? Yo!"),
                            new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Been there for awhile?", new DialogueButton[3]{
                                new DialogueButton("Hey! I'm not some thin air!", 6, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[4] {
									new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "Easy man!"),
									new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "Are you mad? I was just too thirsty to notice you."),
	                                new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "Chill. Didn't mean to offend you."),
                                    new DialogueData(Resources.AvatarDatabase.AndyMadAvatar, "Sometimes it's so annoying that you get offended with small stuff like that.")
								}),
								new DialogueButton("Not really, just got here." + "Wanna grab something to drink?", 1, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
									new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Nah, I'll pass!"),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "As much as I want to buy a drink,"),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "I have to wait here for the rest of the crew to come."),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "I'm waiting for about 5 people since the meeting spot's here, haha!")
								}),
                                new DialogueButton("Maybe.", 6, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[3] {
									new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "That's just creepy. I mean I noticed you staring and all"),
                                    new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "You're creeping me out a bit..."),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "As much as I want to leave, but I'm waiting for some people.")
								})
							})
						}),
					# endregion Dialogue 6

					# region Dialogue 7
						new NPCDialogue(new DialogueData[1] {
							new DialogueData(Resources.AvatarDatabase.AndyMadAvatar, "Really? Like REALLY?" , new DialogueButton[3] {
								new DialogueButton("Someone needs to calm down.", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[8] {
									new DialogueData(Resources.AvatarDatabase.AndyMadAvatar, "Ugh, I'm trying to be."),
                                    new DialogueData(Resources.AvatarDatabase.AndyMadAvatar, "Just... wait."),
                                    new DialogueData(Resources.AvatarDatabase.AndySadAvatar, "..."),
                                    new DialogueData(Resources.AvatarDatabase.AndySadAvatar, "I'm a bit fine now. "),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "I can put on my business smile now, too. Haha!"),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Something personal happened so..."),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "I don't think I'm obligated to tell you."),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "It's just something small. I'll be fiiiiine.")
								}),
                                new DialogueButton("You look awfully...mad...at something.", 7, ButtonType.Wrong, KeypressLevel.Level3, new DialogueData[4] {
									new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "No no, I'm not mad."),
                                    new DialogueData(Resources.AvatarDatabase.AndyMadAvatar, "NOT. AT. ALL."),
                                    new DialogueData(Resources.AvatarDatabase.AndyMadAvatar, "Be more sensitive and just leave me alone."),
                                    new DialogueData(Resources.AvatarDatabase.AndyMadAvatar, "Unless you want to add up to my stress and irritation.")
								}),
                                new DialogueButton("What's wrong with you?", 7, ButtonType.Wrong, KeypressLevel.Level3, new DialogueData[2] {
									new DialogueData(Resources.AvatarDatabase.AndySadAvatar, "Nothing."),
                                    new DialogueData(Resources.AvatarDatabase.AndySadAvatar, "Don't ask. You're not involved so just go.")
								})
							})
						}),
					# endregion Dialogue 7

					# region Dialogue 8
						new NPCDialogue(new DialogueData[4] {
							new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "I could've sworn I fell over there."),
                            new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "Maybe it fell off in a wrong place?"),
                            new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "I just hope it's not lost forever."),
                            new DialogueData(Resources.AvatarDatabase.AndySadAvatar, "I have a lot of important files in there...", new DialogueButton[3] {				
                                new DialogueButton("Maybe you were using your tongue and not your eyes?", 8, ButtonType.Wrong, KeypressLevel.Level4, new DialogueData[4]{
                                    new DialogueData(Resources.AvatarDatabase.AndyMadAvatar, "I've been searching for it EVERYWHERE."),
                                    new DialogueData(Resources.AvatarDatabase.AndyMadAvatar, "Don't suggest that I'm not looking with my eyes!"),
                                    new DialogueData(Resources.AvatarDatabase.AndyMadAvatar, "Ugh! Nevermind. Arguing with you is not helping me find my USB."),
                                    new DialogueData(Resources.AvatarDatabase.AndyMadAvatar, "I'm leaving. You're wasting my precious time from the search.")
                                }),
                                new DialogueButton("How about the lost and found?", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3]{
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Good idea! I haven't checked there yet."),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Thanks a lot, man!"),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "I owe you.")
                                }),
                                new DialogueButton("You should've created some backup files for them.", 8, ButtonType.Wrong, KeypressLevel.Level4, new DialogueData[4]{
                                    new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "That WAS the backup"),
                                    new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "I just don't want anyone else to have an access to them"),
                                    new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "If you'll excuse me,"),
                                    new DialogueData(Resources.AvatarDatabase.AndyMadAvatar, "I have other better things to do than talk to you.")
                                })
							})
						}),
					# endregion Dialogue 8

					# region Dialogue 9
						new NPCDialogue(new DialogueData[3] {
							new DialogueData(Resources.AvatarDatabase.AndySadAvatar, "I hate bringing my laptop from home."),
                            new DialogueData(Resources.AvatarDatabase.AndySadAvatar, "It's one of the reasons why "),
                            new DialogueData(Resources.AvatarDatabase.AndySadAvatar, "...I have second thoughts on the overnight during the event", new DialogueButton[3] {
								new DialogueButton("Why? It's not as hassle as a desktop right?", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5]{
                                    new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "It might not be the same hassle level as a desktop."),
                                    new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "But, it's still hassle, because I have to remove the wires"),
                                    new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "...and mess with my setup at home."),
                                    new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "When I come back after the event,"),
                                    new DialogueData(Resources.AvatarDatabase.AndySadAvatar, "I have to wire it up again and setup it up with my second monitor.")
                                }),
								new DialogueButton("If it's such a hassle, then don't go overnight.", 9, ButtonType.Wrong, KeypressLevel.Level5, new DialogueData[4]{
                                    new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "Do you want to make my teammates lose?"),
                                    new DialogueData(Resources.AvatarDatabase.AndyMadAvatar, "I need as much time as I can to finish the program!"),
                                    new DialogueData(Resources.AvatarDatabase.AndyMadAvatar, "And I can't program efficiently when I'm time pressured!"),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "So please, don't suggest something like that to me anymore.")
                                }),
                                new DialogueButton("Don't participate then if you can't sacrifice.", 9, ButtonType.Wrong, KeypressLevel.Level5, new DialogueData[4]{
                                    new DialogueData(Resources.AvatarDatabase.AndyMadAvatar, "I can sacrifice thank you very much!"),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "You don't tell me what to do, okay?"),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "You know what? Don't butt-in my business anymore and perhaps..."),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "It's time for you to go away?")
                                })
							})
						}),
					# endregion Dialogue 9

					# region Dialogue 10

                        // level6 mini game pls
						new NPCDialogue(new DialogueData[4] {
							new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "I'm sure that kid's getting hyped up over and over again."),
                            new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Hopefully she doesn't annoy or weird out more people."),
                            new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "The only relieving thing she does is that..."),
                            new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "She's prepared like a good little girl scout she always is.", new DialogueButton[3] {
								new DialogueButton("What kid?", ButtonType.Correct, new Statistics(10, 0), new DialogueData[6]{
                                    new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "She's not actually a kid."),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "She's in the same age group as us. She's a designer but, sometimes,"),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "she has to play other roles at the same time too if needed."),
                                    new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "Even if that's the case though, I don't get how she's so hyped up."),
                                    new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "She's so hyped up that I don't even know if it's normal or "),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "...is it just really me being too calm?")
                                }),
                                new DialogueButton("Are you prepared yourself?", ButtonType.Correct, new Statistics(10, 0), new DialogueData[7]{
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "I am actually, but, I don't have to jump up and down to show that."),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "How can she be so talkative when the topic's in her interest"),
                                    new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "And at the same time shy, when talking to strangers."),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Anyways, hmm, I have my stuff ready and packed. Check."),
                                    new DialogueData(Resources.AvatarDatabase.AndyWeirdAvatar, "I have snacks? I guess. And..."),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "The softwares I need are already in my laptop so...Check."),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "I guess I am ready. Hahaha! Just double-checking.")
                                }),
								new DialogueButton("If you only see the wrong in her," + "then shouldn't you stop being friends with her?", ButtonType.Wrong, KeypressLevel.Level5, new DialogueData[5]{
                                    new DialogueData(Resources.AvatarDatabase.AndySadAvatar, "If you're only looking for the good points in people"),
                                    new DialogueData(Resources.AvatarDatabase.AndySadAvatar, "And that's the only basis you have to look at when looking for a friend,"),
                                    new DialogueData(Resources.AvatarDatabase.AndyMadAvatar, "Then, you don't deserve the friends you have now."),
                                    new DialogueData(Resources.AvatarDatabase.AndyHappyAvatar, "Other than that, please refrain from telling me to stop being friends with her."),
                                    new DialogueData(Resources.AvatarDatabase.AndyMadAvatar, "You don't have the right to!")
                                })
							})
						}),
					# endregion Dialogue 10

					},
					new SympathyText(
						new string[4] { "No offense!", "Just kidding, right?", "Sorry", "I didn't mean to." }
					),
                    new ItemAcceptDialogue[1] {
						new ItemAcceptDialogue(
						new DialogueData[1] { 
							new DialogueData("Oh, thank you for handing me this") 
						})
					},
                    new ItemDeclineDialogue[1] {
						new ItemDeclineDialogue(
						new DialogueData[1] { 
							new DialogueData("Um, it's not mine. Sorry, I don't need this") 
						})
					});
                # endregion Andy

                # region Jenevieve
                npcDataInfoList[4] = new NPCData(
					"Jen",
                    "Jenevieve Ashworth",
                    Resources.AvatarDatabase.JenevieveAvatar,
                    NPCNameID.Jenevieve,
                    "A determined 3D Artist, who wants to improve herself in the specific specialty of 3D Art. She likes refreshing cold drinks and listening to music.",
                    new Statistics(87, 76, 34, 72),
                    new ItemsNeeded[2] {
						new ItemsNeeded(ItemNameID.WhiteEarphones), new ItemsNeeded(ItemNameID.IcedTea)
					},
                    new NPCDialogue[10] {

					# region Dialogue 1
						new NPCDialogue(
							new DialogueData[1] {
								new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "Hey. Have you seen any earphones around?", new DialogueButton[2] {
                                    new DialogueButton("Haven’t seen one. Try somewhere else.", 5, ButtonType.Wrong, KeypressLevel.Level1, new DialogueData[3] {
										new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "You don't have to be so rude."),
                                        new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "I was just asking."),
                                        new DialogueData(Resources.AvatarDatabase.JenevieveMadAvatar, "Much bad vibes, how annoying.")
									}),
									new DialogueButton("Um, I don’t think so.", 1, ButtonType.Correct, new Statistics(10, 0), new DialogueData[6] {
										new DialogueData(Resources.AvatarDatabase.JenevieveMadAvatar, "Dammit. Well, if you somehow find it..hmm..."),
                                        new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "Ah, if you find it, just give it to me when you see me next time?"),
                                        new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "You're in the same course as me, right?"),
										new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "Erm, or you can just give it to the lost and found."),
                                        new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "Just tell the lost and found that I own it."),
                                        new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "My name's Jenevieve by the way.")
									})
								})
							}
						),
					# endregion Dialogue 1

					# region Dialogue 2
						new NPCDialogue(
							new DialogueData[4] {
                                new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "Oh, I give up!"),
                                new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "I’ll check it out at the lost and found later."),
                                new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "Come to think of it. I’ve never seen you around before."),
								new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "Who’re you?", new DialogueButton[2] {
                                    new DialogueButton("I'm busy.", 6, ButtonType.Wrong, KeypressLevel.Level1, new DialogueData[3] {
										new DialogueData(Resources.AvatarDatabase.JenevieveMadAvatar, "Really? Just...really?"),
										new DialogueData(Resources.AvatarDatabase.JenevieveMadAvatar, "You know what? Nevermind. "),
                                        new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "Geez, I was only trying to be friendly.")
									}),
									new DialogueButton("Someone not that important," + "but been around for some time.", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3] {
										new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "I can't seem to grab a hold of what you meant"),
										new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "Are you depressed or is this normally you?"),
										new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "Anyways, nice meeting you..")
									})		
								})
							}
						),
					# endregion Dialogue 2

					# region Dialogue 3
						new NPCDialogue(
							new DialogueData[3] {
								new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "It’s you again?"),
								new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "Well, I shouldn’t be surprised, right?"),
								new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "We ARE in the same course.", new DialogueButton[2] {
                                    new DialogueButton("We might've been in the same classes the previous terms" +
                                                        "but, I was thin air to you so don't bother.", 7, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[5] {
										new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "Sorry if I offended you or anything..."),
                                        new DialogueData(Resources.AvatarDatabase.JenevieveMadAvatar, "But, that way of talking doesn't sit well with me."),
                                        new DialogueData(Resources.AvatarDatabase.JenevieveMadAvatar, "Got that?"),
                                        new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "No wonder people didn't approach you."),
										new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "With that kind of attitude?")
									}),
									new DialogueButton("I'm surprised, you even noticed me.", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5] {
										new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "Hey, I remembered you."),
										new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "So, don't be so down."),
                                        new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "I'm sure more people will recognize you sooner or later."),
                                        new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "It's not about recognition though,"),
                                        new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "...it's more about making friends, okay?")
									})				
								})

						}),
					# endregion Dialogue 3

					# region Dialogue 4
						new NPCDialogue(new DialogueData[3] {
                            new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "Do you know any Franz, Maxine, Noelle, Andy or Bart?"),
							new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "We’re supposed to have a meet up somewhere."),
                            new DialogueData(Resources.AvatarDatabase.JenevieveMadAvatar, "Ugh, isn’t it too much to ask for them to reply where they are?", new DialogueButton[3] {
								new DialogueButton("Maybe you should call one of them instead?", 4, ButtonType.Correct, new DialogueData[7] {
									new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "Yeah, I could do that..."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "if I had that much load to call them one by one."),
									new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "Thanks for the suggestion though."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "Oh, wait."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "Finally! Someone replied!"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "Wait, it's not a reply, it's a phonecall"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "I gotta go answer this. See you!")
								}),
                                new DialogueButton("You should just go straight to your meet up place.", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5] {
									new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "But what if they're not there?"),
									new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "Wait, why am I having second thoughts?"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "Of course, Noelle and Andy are gonna be there."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "They're always early."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "Guess this means, I gotta jet. See you around!")
								}),
								new DialogueButton("Familiar names, but no." + "Now, go along and find your friends.", 8, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[1] {
									new DialogueData(Resources.AvatarDatabase.JenevieveMadAvatar, "Jerk.")
								})
							})
						}),
					# endregion Dialogue 4

					# region Dialogue 5
						new NPCDialogue(new DialogueData[1] {
							new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "Are you participating?", new DialogueButton[3] {
								new DialogueButton("Yeah, I am! I’m excited and sad about it," +
                                                    "It’s my first and last time participating as a student!", 9, ButtonType.Correct, new Statistics(10, 0), new DialogueData[8] {
									new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "Oh, you didn’t participate in the previous years?"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "I don’t know if it would be fun for you or not."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "I had fun for spending time and moments with friends."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "They were my teammates back then."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "It was tiring as hell though!"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "Trying to stay awake for how many hours for 3 days and two nights straight?"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "I don’t even know if I want a repeat of that again."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "Hmm, it wasn’t that bad at the least, so, you might see me there? I guess?")
								}),
                                new DialogueButton("In what?", 9, ButtonType.Wrong, KeypressLevel.Level3, new DialogueData[2] {
									new DialogueData(Resources.AvatarDatabase.JenevieveMadAvatar, "In the game jam, duh?"),	
                                    new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "How could you not know that?")
								}),
                                new DialogueButton("Yeah, I guess I'm a bit prepared for it. Maybe.", 9, ButtonType.Correct, new Statistics(10, 0), new DialogueData[6] {
									new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "It's better than nothing right?"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "If you prepared at least something, you have an advantage already"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "So don't let nervousness get to you, alright? Calm down."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "Though, I can't say the same for myself."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "I'm nervous too, but at the same time, kind of excited."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "I wonder what game I'll be able to create for this year...")
								})
								
							})
						}),
					# endregion Dialogue 5

					# region Dialogue 6
						new NPCDialogue(new DialogueData[6] {
                            new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "Ugh, I could use some noodles right now..."),
							new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "No.. no. I want some green mangoes"),
                            new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "Or maybe some iced tea?"),
                            new DialogueData(Resources.AvatarDatabase.JenevieveMadAvatar, "Ah, I suddenly remembered last night's tv show! Darn it!"),
                            new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "Now, I want some burger or steak!"),
                            new DialogueData(Resources.AvatarDatabase.JenevieveMadAvatar, "So frustrating...!", new DialogueButton[3]{						
								new DialogueButton("Are you pregnant or something?", 6, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[5] {
									new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "What?!"),
									new DialogueData(Resources.AvatarDatabase.JenevieveMadAvatar, "You don't ask a woman a question like that."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveMadAvatar, "Just because I craved a little..."),
	                                new DialogueData(Resources.AvatarDatabase.JenevieveMadAvatar, "YES a LITTLE, it doesn't automatically mean I'm pregnant!"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveMadAvatar, "Ugh! How annoying.")
								}),
                                new DialogueButton("Are you buying for all of those?", 6, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[6] {
									new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "Well, duh! I won't buy for all of those."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "And of course, I can't finish all of those if I had them"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "That's just my tongue and stomach talking."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "Also, it's not like I have that much money on me."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "Obviously, that's one of the reasons why I'm frustrated."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "I don't have enough money for any of it!")
								}),
                                new DialogueButton("How come you have those cravings?", 1, ButtonType.Correct, new Statistics(10, 0), new DialogueData[2] {
									new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "Don't mind me."),
									new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "I'm just being my normal self haha!")
								})
							})
						}),
					# endregion Dialogue 6

					# region Dialogue 7
						new NPCDialogue(new DialogueData[1] {
							new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "You hungry?", new DialogueButton[3] {			
                                new DialogueButton("What could be more important than food?", 7, ButtonType.Wrong, KeypressLevel.Level3, new DialogueData[3] {
									new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "I don't know. Money? Family?"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "Obviously, a lot of things are!"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "Don't ask about obvious things, please!")
								}),
								new DialogueButton("Thanks, but no thanks.", 7, ButtonType.Wrong, KeypressLevel.Level3, new DialogueData[4] {
									new DialogueData(Resources.AvatarDatabase.JenevieveMadAvatar, "Don't force your stomach to go quiet!"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "If you're hungry then you should eat okay?"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "I'm not the one to talk though."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "I usually skip my breakfast.")
								}),
                                new DialogueButton("Just a little. I guess?" + "Hm, here have a drink...", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[1] {
									new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "Thanks for the hospitality, but, I have to go though.")
								})
                              
							})
						}),
					# endregion Dialogue 7

					# region Dialogue 8
						new NPCDialogue(new DialogueData[3] {
							new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "You don't have any class, do you?"),
                            new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "From morning until now, I only saw you doing one thing."),
                            new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "Over and over.", new DialogueButton[3] {
								new DialogueButton("I don't have any actually." + "Does that strike odd to you?", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[2]{
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "No, not really."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "I'm just surprised you can last this long in school doing nothing.")
                                }),
								new DialogueButton("You think?", 8, ButtonType.Wrong, KeypressLevel.Level4, new DialogueData[3]{
                                    new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "I was just pointing out a fact."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveMadAvatar, "You don't have to be mad!"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "Geez!")
                                }),
                                new DialogueButton("You're accusing me of stalking, aren't you?", 8, ButtonType.Wrong, KeypressLevel.Level4, new DialogueData[2]{
                                    new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "No, I don't."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveMadAvatar, "Seriously, I was just asking!")
                                })
							})
						}),
					# endregion Dialogue 8

					# region Dialogue 9
						new NPCDialogue(new DialogueData[2] {
							new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "Sigh, just thinking about it makes my back ache."),
                            new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "Such a hassle.", new DialogueButton[3] {
								new DialogueButton("What's a hassle?", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5]{
                                    new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "Oh you know, bringing the laptop and at the same time"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "...bringing your clothes."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveMadAvatar, "It's only one back pack, but, it's too heavy!"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "But, it's one of the things I figure I have to endure"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "if I want to participate and enjoy the jam.")
                                }),
                                new DialogueButton("Then, don't think about it.", 9, ButtonType.Wrong, KeypressLevel.Level5, new DialogueData[2]{
                                    new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "Even if I don't, I'll still experience it anyway, obviously"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "Sigh.")
                                }),
								new DialogueButton("If it's such a hassle, why do it?", 9, ButtonType.Wrong, KeypressLevel.Level5, new DialogueData[1]{
                                    new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "Didn't you even wonder what I was doing and why I was doing it?")
                                })
                                
							})
						}),
					# endregion Dialogue 9

					# region Dialogue 10
                        // level6 mini game pls
						new NPCDialogue(new DialogueData[5] {
							new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "Since you're joining the game jam,"),
                            new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "are you more of an artist or a programmer?"),
                            new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "I'm an artist but I lean more on 3D."),
                            new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "I'm not recruiting or anything for a premade team, don't misunderstand"),
                            new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "It's just a simple question.", new DialogueButton[3] {
								new DialogueButton("If that's the case then, I'm more of a programmer.", ButtonType.Correct, new Statistics(10, 0), new DialogueData[6]{
                                    new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "That's new."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "I can't believe there's another programmer in our batch"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "I only know a few."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "Programmers are quite rare, not during the game jam though."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "There are others beside our school participating after all."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "It's not going to be easy, but, hey... experience right?")
                                }),
                                new DialogueButton("If that's the case then, I'm more of an artist.", ButtonType.Correct, new Statistics(10, 0), new DialogueData[2]{
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "Oh the same as me then!"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveHappyAvatar, "Welcome aboard to the game jam artists' crew hahaha!")
                                }),
								new DialogueButton("I'm flexible...I think? Hehe", ButtonType.Wrong, KeypressLevel.Level5, new DialogueData[5]{
                                    new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "Please! Be more specific!"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "Ugh, 'cause if you don't know what role you are,"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "you won't know what you should contribute."),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveSadAvatar, "Have mercy on your teammates!"),
                                    new DialogueData(Resources.AvatarDatabase.JenevieveWeirdAvatar, "There's time pressure you know that right?")
                                })
							})
						}),
					# endregion Dialogue 10

					},
					new SympathyText(
						new string[4] { "Sorry.", "Ah! I didn't mean it!", "I apologize.", "No offense!" }
					),
					new ItemAcceptDialogue[1] {
						new ItemAcceptDialogue(
						new DialogueData[1] { 
							new DialogueData("Just what I need! Thank you for giving me this") 
						})
					},
					new ItemDeclineDialogue[1] {
						new ItemDeclineDialogue(
						new DialogueData[1] { 
							new DialogueData("I actually don't need that") 
						})
					});
                # endregion Jenevieve

                # region Franz
                npcDataInfoList[5] = new NPCData(
					"Franz",
                    "Franzine Vespermann",
                    Resources.AvatarDatabase.FranzAvatar,
                    NPCNameID.Franz,
                    "A creative 2D and 3D Artist, who is willing to self-study to improve her skills. She's a supporter of natural and healthy drinks and is currently in love with Traditional Art.",
                    new Statistics(90, 77, 30, 65),
                    new ItemsNeeded[2] {
						new ItemsNeeded(ItemNameID.Markers), new ItemsNeeded(ItemNameID.OrangeJuice)
					},
                    new NPCDialogue[10] {

					# region Dialogue 1
						new NPCDialogue(
							new DialogueData[1] {
								new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "Hi… I’m Franzine, but, um… uh, you can just call me Franz.", new DialogueButton[2] {
									new DialogueButton("H-Hi! Um…", 1, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3] {
										new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "Are you nervous? Don’t be!"),
                                        new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "I-I’ll get nervous too."),
                                        new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "It's nice to meet you though!")
									}),
									new DialogueButton("Cool you have a nickname!", 5, ButtonType.Wrong, KeypressLevel.Level1, new DialogueData[2] {
										new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "Franzine's a long name so...yeah"),
                                        new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "That's basically it though.")
									})
								})
							}
						),
					# endregion Dialogue 1

					# region Dialogue 2
						new NPCDialogue(
							new DialogueData[1] {
                                new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "A-Are you an artist or…?", new DialogueButton[2] {
									new DialogueButton("I’m a bit of both but, I’m more artsy.", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3] {
										new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "Um, do you like manga and anime?"),
                                        new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "I'm not assuming that all artists have to like them."),
                                        new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "I.. just wanna ask. Hehe")
									}),
									new DialogueButton("I don’t know.", 6, ButtonType.Wrong, KeypressLevel.Level1, new DialogueData[8] {
										new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "Oh so you're not sure yourself yet then?"),
                                        new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "That's a bit worrisome..."),
                                        new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "You should decide soon."),
                                        new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "Just a little piece of advice..."),
                                        new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "Um.. I'm not telling you to actually do this."),
                                        new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "This is just an advice, but,"),
                                        new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "if you have tried every role already available."),
                                        new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "Then why not be in the role you enjoyed the most in, right?")
									})
								})
							}
						),
					# endregion Dialogue 2

					# region Dialogue 3
						new NPCDialogue(
							new DialogueData[7] {
                                new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "Have you tried using watercolors and markers before?"),
                                new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "It’s really fun and interesting with what you can do with those!"),
                                new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "I suggest you try a lot of things with traditional"),
                                new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "...if you’re tired for doing digital for awhile."),
                                new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "I mean don’t just sketch and color with pencils."),
                                new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "I-I’m not forcing you or anything."),
								new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "It’s just a suggestion.", new DialogueButton[2] {
                                    new DialogueButton("Traditional's not that good. It's kind of lame.", 7, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[3] {
										new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "It's your opinion. I can't do anything about it."),
                                        new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "Just wanted to broaden your horizons,"),
                                        new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "But if you don't want any of it...then okay.")
									}),
									new DialogueButton("That sounds like a cool suggestion. I just might try it!", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5] {
										new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "Do prepare to spend a lot though."),
                                        new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "The prices for different art sets like..."),
                                        new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "markers, color pencils, water brushes and watercolor paint are no joke..."),
                                        new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "especially when you have to constantly refill some of them."),
                                        new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "It’s worth it though!")
									})
									
								})

						}),
					# endregion Dialogue 3

					# region Dialogue 4
						new NPCDialogue(new DialogueData[5] {
                            new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "Speaking of markers…"),
                            new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "I can’t seem to find my set."),
                            new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "I really didn’t spend anything for those,"),
                            new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "but, all the more reason to find them."),
                            new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "They’re gifts from friends of mine.", new DialogueButton[3] {
								new DialogueButton("Try tracing back your steps.", 4, ButtonType.Correct, new DialogueData[3] {
									new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "I can't remember everything but, I'll do my best."),
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "I mean I have to...if I want to get my markers back."),
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "Thanks for giving me an idea on what to do.")
								}),
								new DialogueButton("You might not find them again." + 
                                                    "They're expensive.", 8, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[3] {
									new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "We should stay positive!"),
                                    new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "Why so negative? There's always a way!"),
                                    new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "Stop making me lose my spirit.")
								}),
                                new DialogueButton("Try in the lost and found?", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[2] {
									new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "Hopefully, the one who found my markers is a good person"),
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "And took it there.")
								})
							})
						}),
					# endregion Dialogue 4

					# region Dialogue 5
						new NPCDialogue(new DialogueData[1] {
							new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "Game Jam's around the corner. I'm a bit anxious." , new DialogueButton[3] {
								new DialogueButton("Why?", 9, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5] {
									new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "Despite that it's fun and all, it IS still a competition."),
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "That doesn't matter to me much though,"),
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "I'm more concerned with my skills."),
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "I mean I prepared by practicing lots of times."),
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "I'm just scared that it might not have an effect.")
								}),
                                new DialogueButton("Don't be! Stay cheerful and let's all do our best!", 9, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5] {
									new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "That sounds a bit relieving."),
                                    new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "The anxiousness is still there but,"),
                                    new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "it's not as bad as it was before."),
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "Thanks..!"),
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "Now to prepare on how to talk to new people.")
								}),
								new DialogueButton("Everyone is; not just you. So stop complaining.", 9, ButtonType.Wrong, KeypressLevel.Level3, new DialogueData[5] {
									new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "What?"),
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "I was just telling you how I feel."),
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "I'm not complaining anything."),
                                    new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "If you don't like me sharing things with you, just tell me!"),
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "I thought we were friends enough to do at least that.")
								})
							})
						}),
					# endregion Dialogue 5

					# region Dialogue 6
						new NPCDialogue(new DialogueData[2] {
                            new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "Yes! It updated and I finally read it."),
                            new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "Gosh, those moments. Hehehe~", new DialogueButton[3]{
                                new DialogueButton("Get over it.", 6, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[4] {
									new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "STOP IT!"),
                                    new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "Stop being so antagonistic!"),
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "What did I ever do to you?"),
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "I just want some peace and enjoy my manga like everyone else...")
								}),
								new DialogueButton("You seem to be enjoying that.", 1, ButtonType.Correct, new Statistics(10, 0), new DialogueData[6] {
									new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "I just can't help it. The updated chapter was just so sweet."),
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "You should try reading it."),
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "Well, if you're interesed in shoujo mangas"),
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "The art's so pretty though especially when it was animated."),
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "The colors and the likes."),
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "So clean, it's a pastel-colored artwork painted using water color.")
								}),
								new DialogueButton("Did someone die already?", 6, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[4] {
									new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "No one's dying."),
                                    new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "No character death in this story, okay?"),
                                    new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "Not that I can control the story...STILL!"),
                                    new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "I just know it has no character death!")
								})
                                
							})
						}),
					# endregion Dialogue 6

					# region Dialogue 7
						new NPCDialogue(new DialogueData[6] {
							new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "Even if I'm an artist,"),
                            new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "it's really difficult to think of designs myself."),
                            new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "Sure there are a lot of reference materials,"),
                            new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "but I want to draw something unique"),
                            new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "that I imagined intricately in my head myself"),
                            new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "and not imagined and designed by others.", new DialogueButton[3] {
								new DialogueButton("Why are you even an artist?", 7, ButtonType.Wrong, KeypressLevel.Level3, new DialogueData[4] {
									new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "Hey!"),
                                    new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "That's crossing the line."),
                                    new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "I'm doing my best to improve on that you know?"),
                                    new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "I work hard for that, okay?!")
								}),
                                new DialogueButton("I wonder if you'd even get clients.", 7, ButtonType.Wrong, KeypressLevel.Level3, new DialogueData[4] {
									new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "You're not helping and you're just pushing me down!"),
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "Please. I don't need those kind of words from you."),
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "I'm trying to lessen those art blocks"),
                                    new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "...so don't imply that I've been doing nothing about it!")
								}),
                                new DialogueButton("Why don't you try reading novels?", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[7] {
									new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "I'm not really the hardcore reading type of person."),
                                    new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "I just read books every now and then..."),
                                    new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "if they're interesting enough."),
                                    new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "But if that would help me like picture out,"),
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "I guess, why not?"),
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "And maybe after reading, I can draw up some scenarios from the book"),
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "and design the characters myself!")
								})
							})
						}),
					# endregion Dialogue 7

					# region Dialogue 8
						new NPCDialogue(new DialogueData[2] {
							new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "There's perspective, conceptualizing..."),
                            new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "There's also shading and detailing...", new DialogueButton[3] {
								new DialogueButton("Just what... are you talking about?", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5]{
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "Ah! Y-You surprised me!"),
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "I didn't see you there!"),
                                    new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "Well, if you're talking about the things I've been saying,"),
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "they're things I want to try to improve on."),
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "I still have a long way to go I guess, hahaha")
                                }),
								new DialogueButton("Are you sure you can master all of those?", 8, ButtonType.Wrong, KeypressLevel.Level4, new DialogueData[5]{
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "I can manage."),
                                    new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "Don't you look down on me!"),
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "It doesn't help with my morale"),
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "if you keep on badgering and looking down on me."),
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "So please stop bothering me...and maybe, go away?")
                                }),
                                new DialogueButton("There's also the digital mastery.", 8, ButtonType.Wrong, KeypressLevel.Level4, new DialogueData[3]{
                                    new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "I know that!"),
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "And I can manage okay?"),
                                    new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "And yes you were implying something that puts me down.")
                                })
							})
						}),
					# endregion Dialogue 8

					# region Dialogue 9
						new NPCDialogue(new DialogueData[1] {
							new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "It's a good thing that my place is nearby the school.", new DialogueButton[3] {
								 new DialogueButton("So what?", 9, ButtonType.Wrong, KeypressLevel.Level5, new DialogueData[4]{
                                    new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "Huh?"),
                                    new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "Why so rude?"),
                                    new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "Ugh, I'll just leave you alone now."),
                                    new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "I was just expressing my thoughts. Hmph.")
                                }),
                                new DialogueButton("Why is that a good thing?", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3]{
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "I don't have to be worried constantly"),
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "just because I'm carrying my laptop and the likes."),
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "And it's less back pain for me when the game jam time comes.")
                                }),
								new DialogueButton("You're making the faraway travellers envious.", 9, ButtonType.Wrong, KeypressLevel.Level5, new DialogueData[4]{
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "T-that's not what I meant..."),
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "I'm just sharing and all..."),
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "And besides, I'm a faraway traveller too "),
                                    new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "if I went home during weekends!")
                                })
                               
							})
						}),
					# endregion Dialogue 9

					# region Dialogue 10

                        // level6 mini game pls
						new NPCDialogue(new DialogueData[3] {
							new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "I wonder if his laptop's going to crash again during game jam"),
                            new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "Hopefully, it won't?"),
                            new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "That'd be a hassle for his teammates and all.", new DialogueButton[3] {
								new DialogueButton("Another laptop crash?", ButtonType.Correct, new Statistics(10, 0), new DialogueData[6]{
                                    new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "Oh, you were listening in?"),
                                    new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "Um, it's just that..."),
                                    new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "he just had his laptop get fixed and uh..."),
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "His laptop always crashes at the wrong time."),
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "And by at the wrong time, I meant"),
                                    new DialogueData(Resources.AvatarDatabase.FranzSadAvatar, "when he badly needs it for school and the likes.")
                                }),
                                new DialogueButton("'His'?", ButtonType.Correct, new Statistics(10, 0), new DialogueData[4]{
                                    new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "If you're wondering who, it's Bart."),
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "If you see a bit of a chubby guy with earrings and black shirt..oh.."),
                                    new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "That's a bit of um.. generic."),
                                    new DialogueData(Resources.AvatarDatabase.FranzHappyAvatar, "Sorry, I don't know how else to describe him.")
                                }),
								new DialogueButton("I'm definitely not going to be his teammate then", ButtonType.Wrong, KeypressLevel.Level5, new DialogueData[9]{
                                    new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "Hey!"),
                                    new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "What did he ever do to you?"),
                                    new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "It was just his laptop and it's a minimal chance anyway!"),
                                    new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "Don't judge him because of his laptop."),
                                    new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "How do you look for teammates then?"),
                                    new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "Just by their use to you?!"),
                                    new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "Well, if that's the case then,"),
                                    new DialogueData(Resources.AvatarDatabase.FranzWeirdAvatar, "I'm definitely NOT going to be on your team if by chance"),
                                    new DialogueData(Resources.AvatarDatabase.FranzMadAvatar, "you invite me. Hmph.")
                                })
							})
						}),
					# endregion Dialogue 10

					},
					new SympathyText(
						new string[4] { "I'm so sorry!", "I-I didn't mean to!", "Please don't mind what I said!", "Please forgive me..." }
					),
					new ItemAcceptDialogue[1] {
						new ItemAcceptDialogue(
						new DialogueData[1] { 
							new DialogueData("Oh, thank you so much for giving me this") 
						})
					},
					new ItemDeclineDialogue[1] {
						new ItemDeclineDialogue(
						new DialogueData[1] { 
							new DialogueData("It's not mine. Sorry for the trouble of giving me that") 
						})
					});
                # endregion Franz

                # region Bart
                npcDataInfoList[6] = new NPCData(
					"Bart",
                    "Bartholomew Beacock",
                    Resources.AvatarDatabase.BartAvatar,
                    NPCNameID.Bart,
                    "A sound engineer with full of humor, who is very much talented with his hearing. He always has his guitar buddy and is currently addicted to caffeine products.",
                    new Statistics(85, 80, 45, 95),
                    new ItemsNeeded[2] {
						new ItemsNeeded(ItemNameID.Guitar), new ItemsNeeded(ItemNameID.HotCoffee)
					},
                    new NPCDialogue[10] {

					# region Dialogue 1
						new NPCDialogue(
							new DialogueData[1] {
								new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Oi, ‘sup!", new DialogueButton[2] {
                                    new DialogueButton("Uh.. hi.", 5, ButtonType.Wrong, KeypressLevel.Level1, new DialogueData[3] {
										new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "..."),
                                        new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "Uh, you're kind of...I don't know..."),
                                        new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "/scratches head/")
									}),
									new DialogueButton("Oh, hi there!", 1, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
										new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "I'm Bartholomew... but yeah that's kinda long so..."),
                                        new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Call me Bart."),
                                        new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "Um, have I seen you around before...?"),
                                        new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Maybe in class. I guess?")
									})	
								})
							}
						),
					# endregion Dialogue 1

					# region Dialogue 2
						new NPCDialogue(
							new DialogueData[7] {
                                new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Just a random thought on the top of my head."),
                                new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Have you ever heard of the Bermuda Triangle?"),
                                new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Legend says a lot of plane crashed and a lot of boats sank there."),
                                new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "What if the reason for those crashes and sinking"),
                                new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "wasn’t really due to the storms and such?"),
                                new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Didn’t you know that the lochness monster is actually true"),
                                new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "and is living in the Bermuda Triangle?" , new DialogueButton[2] {
									new DialogueButton("I know all about the Bermuda Triangle." + "I do my own fair research about general things", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
										new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "Oh..."),
                                        new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "You got me good, hahaha!"),
                                        new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "I guess you're another person not to joke on about those things since"),
                                        new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "you might have a rebuttal for the 'facts' I'm about to say next. hahaha!")
									}),
									new DialogueButton("What kind of nonsense are you talking about?", 6, ButtonType.Wrong, KeypressLevel.Level1, new DialogueData[4] {
										new DialogueData(Resources.AvatarDatabase.BartSadAvatar, "It’s not nonsense."),
                                        new DialogueData(Resources.AvatarDatabase.BartSadAvatar, "I was only trying to inform you."),
                                        new DialogueData(Resources.AvatarDatabase.BartSadAvatar, "Alright, alright."),
                                        new DialogueData(Resources.AvatarDatabase.BartSadAvatar, "You don’t really care so I’ll be going now.")
									})
								})
							}
						),
					# endregion Dialogue 2

					# region Dialogue 3
						new NPCDialogue(
							new DialogueData[1] {
								new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Knock knock!", new DialogueButton[2] {
									new DialogueButton("Pft. Who’s there?", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[2] {
										new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "Actually… I forgot what my joke was."),
                                        new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Hahaha! Sorry, sorry.")
									}),
									new DialogueButton("I’m not riding on that joke.", 7, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[4] {
										new DialogueData(Resources.AvatarDatabase.BartMadAvatar, "Why are you such a killjoy?"),
                                        new DialogueData(Resources.AvatarDatabase.BartMadAvatar, "I just want to have fun and maybe"),
                                        new DialogueData(Resources.AvatarDatabase.BartMadAvatar, "make everyone else here laugh!"),
                                        new DialogueData(Resources.AvatarDatabase.BartSadAvatar, "Tch. Fine. Nevermind then.")
									})
								})

						}),
					# endregion Dialogue 3

					# region Dialogue 4
						new NPCDialogue(new DialogueData[1] {
                            new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Pfft. Hahaha did you just fart?", new DialogueButton[3] {
                                new DialogueButton("Nope, if I had been the one," + "it would’ve been more deadly than this. Hahaha!", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
									new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Hahaha! That's too good to be true!"),
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "You know what? You're quite funny."),
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "To be honest, I didn't know you had it in you."),
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "You just don't seem the type. hahaha!")
								}),
								new DialogueButton("If you were the one who asked first," + "then you’re the one who farted. Haha", 4, ButtonType.Correct, new DialogueData[3] {
									new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "That maybe true."),
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "But, you WERE quiet for some time. Haha!"),
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Pfft. Just... hahaha! admit it!")
								}),
                                new DialogueButton("Don’t frame me for something you did!", 8, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[2] {
									new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "Oi, chill!"),
                                    new DialogueData(Resources.AvatarDatabase.BartMadAvatar, "I was just kidding, man!")
								})
							})
						}),
					# endregion Dialogue 4

					# region Dialogue 5
						new NPCDialogue(new DialogueData[4] {
                            new DialogueData(Resources.AvatarDatabase.BartSadAvatar, "I’m kind of worried about game jam"),
                            new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "Well, I’m not really worried about the competition and all."),
                            new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "I’m not even worried about the hassle of travelling with a laptop."),
							new DialogueData(Resources.AvatarDatabase.BartSadAvatar, "What I’m worried about is that if my laptop will survive the game jam." , new DialogueButton[3] {
                                new DialogueButton("Then, why join at all?", 9, ButtonType.Wrong, KeypressLevel.Level3, new DialogueData[1] {
                                    new DialogueData(Resources.AvatarDatabase.BartMadAvatar, "Could you not assume things?")
								}),
								new DialogueButton("Survive...that's one heavy word" + "for the condition of your laptop.", 9, ButtonType.Correct, new Statistics(10, 0), new DialogueData[6] {
									new DialogueData(Resources.AvatarDatabase.BartSadAvatar, "Well, it has that one habit of crashing, "),
                                    new DialogueData(Resources.AvatarDatabase.BartSadAvatar, "EVERY TIME I use it on something important.. "),
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "And well, game jam is important and at the same time, time pressured."),
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "So every ticking second shouldn’t be wasted"),
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "with much reboots on my laptop if it ever crashes.."),
                                    new DialogueData(Resources.AvatarDatabase.BartSadAvatar, "Sigh.")
								}),
                                new DialogueButton("If you had your laptop fixed before," + "then it won't crash anytime soon!", 9, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
									new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "I guess I just have to think positive and maybe, it won’t crash?"),
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "But just in case, I might have to do an every hour backup thing."),
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "Just in case it won’t reboot anymore because of multiple crashes."),
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "Such a tedious process.")
								})
								
							})
						}),
					# endregion Dialogue 5

					# region Dialogue 6
						new NPCDialogue(new DialogueData[1] {
                            new DialogueData(Resources.AvatarDatabase.BartSadAvatar, "Sigh, I was so sure I left my guitar by the lobby.", new DialogueButton[3]{
                                new DialogueButton("Why do you even have a guitar with you anyways?", 6, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[4] {
									new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "Maybe, I just want to bring to it with me?"),
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "Oh, you never know, okay?"),
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "And it’s not like..."),
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "there’s a rule about bringing guitars in school.")
								}),
								new DialogueButton("Why? And wait, you play guitar?", 1, ButtonType.Correct, new Statistics(10, 0), new DialogueData[6] {
									new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Yeah, I have a band with my friends."),
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "And about the guitar, I brought it this morning."),
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "And tried playing it in the lobby."),
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "But, when I was called back in the classroom"),
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "and then came back to the lobby,"),
                                    new DialogueData(Resources.AvatarDatabase.BartSadAvatar, "it wasn't there anymore.")
								}),
                                new DialogueButton("Someone took it and will never give it back to you ever.", 6, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[3] {
									new DialogueData(Resources.AvatarDatabase.BartMadAvatar, "Impossible!"),
                                    new DialogueData(Resources.AvatarDatabase.BartMadAvatar, "That's too big to not notice."),
                                    new DialogueData(Resources.AvatarDatabase.BartSadAvatar, "Stop making me think of negative things, please.")
								})
							})
						}),
					# endregion Dialogue 6

					# region Dialogue 7
						new NPCDialogue(new DialogueData[4] {
							new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "I think I'm getting addicted with hot coffee lately"),
                            new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "I know it's bad but, I can't help it."),
                             new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "I needed it to stay awake."),
                            new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "And now that I'm joining game jam, it will just get worse.", new DialogueButton[3] {
								new DialogueButton("It IS still your choice you know?" + "You can drink something else.", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5] {
									new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "True."),
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "OR I could've used snacks"),
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "instead of coffee to keep me awake."),
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Why didn't I think of that"),
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "...before I got a bit addicted?")
								}),
                                 new DialogueButton("It won't get worse if you CHOSE not to drink it.", 7, ButtonType.Wrong, KeypressLevel.Level3, new DialogueData[2] {
									new DialogueData(Resources.AvatarDatabase.BartSadAvatar, "But, it's really good and it really helps me stay awake!"),
                                    new DialogueData(Resources.AvatarDatabase.BartSadAvatar, "And there's no other drink that could help me stay awake!")
								}),
								new DialogueButton("And that's why I never drink coffee!", 7, ButtonType.Wrong, KeypressLevel.Level3, new DialogueData[1] {
									new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "Your point what exactly...?")
								})
							})
						}),
					# endregion Dialogue 7

					# region Dialogue 8
						new NPCDialogue(new DialogueData[1] {
							new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "I'm hungry. I wonder what should I eat?", new DialogueButton[3] {					
								new DialogueButton("Pasta?", 8, ButtonType.Wrong, KeypressLevel.Level4, new DialogueData[2]{
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "Not really feeling the pasta right now..."),
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Go eat some if you want to though!")
                                }),
                                new DialogueButton("How about noodles?", 8, ButtonType.Wrong, KeypressLevel.Level4, new DialogueData[2]{
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "It's pretty hot outside and you want noodles?"),
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "Well, it's not that bad of a choice, but, it is STILL noodles.")
                                }),
                                new DialogueButton("Just get whatever you feel like eating?", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[6]{
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Then, let's get burgers!"),
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "Which fastfood chain though..."),
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "It's really hard to pick what food to eat."),
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "But, first!"),
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Let me check my wallet hahaha!"),
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Oh. I think... I'll just go for siomai rice. Sigh. Hahaha!")
                                })
							})
						}),
					# endregion Dialogue 8

					# region Dialogue 9
						new NPCDialogue(new DialogueData[2] {
							new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Do you want to find out my role in the game jam and projects?"),
                            new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "You'll be surprised!", new DialogueButton[3] {
								new DialogueButton("I don't think I'll be surprised at all.", 9, ButtonType.Wrong, KeypressLevel.Level5, new DialogueData[4]{
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "Oh c'mon!"),
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "Can't you even pretend to be surprised if ever?"),
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "Ugh, you're so frustrating."),
                                    new DialogueData(Resources.AvatarDatabase.BartMadAvatar, "Now, I don't feel like telling you anymore.")
                                }),
                                new DialogueButton("Sure, go ahead and surprise me. Hahaha!", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3]{
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "I'm a sound engineer!"),
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Well, part-time 3D animator, too"),
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "But yeah, I do sounds! Were you surprised? hahaha!")
                                }),
                                new DialogueButton("I'm not really that interested in knowing.", 9, ButtonType.Wrong, KeypressLevel.Level5, new DialogueData[2]{
                                    new DialogueData(Resources.AvatarDatabase.BartMadAvatar, "Meh, suit yourself."),
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "Such a killjoy.")
                                })
							})
						}),
					# endregion Dialogue 9

					# region Dialogue 10

                        // level6 mini game pls
						new NPCDialogue(new DialogueData[4] {
							new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "I guess I can say I'm ready."),
                            new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "How about you? Are you ready?"),
                            new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "'Cause I'm ready! So..."),
                            new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Are you really ready?", new DialogueButton[3] {
								new DialogueButton("I'm ready. I'm ready. I'M READY!", ButtonType.Correct, new Statistics(10, 0), new DialogueData[3]{
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "That's the spirit! Hahaha!"),
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Good luck staying up in the upcoming 3 days and 2 nights."),
                                    new DialogueData(Resources.AvatarDatabase.BartSadAvatar, "And good luck for my laptop for the next 3 days and 2 nights.")
                                }),
                                new DialogueButton("I am! Are you ready? 'Cause I'm ready." + "You should be ready!", ButtonType.Correct, new Statistics(10, 0), new DialogueData[4]{
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Hahaha I wonder how much ready we've said in this conversation."),
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "But seriously, I am ready."),
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "I just hope my teammates-to-be won't decide on 3D though..."),
                                    new DialogueData(Resources.AvatarDatabase.BartSadAvatar, "I really don't know how much my laptop could take anymore.")
                                }),
								new DialogueButton("Do you have to ask the same question over and over again?!", ButtonType.Wrong, KeypressLevel.Level5, new DialogueData[2]{
                                    new DialogueData(Resources.AvatarDatabase.BartHappyAvatar, "Hahaha! Why so mad?"),
                                    new DialogueData(Resources.AvatarDatabase.BartWeirdAvatar, "I was just kidding, seriously.")
                                })
							})
						}),
					# endregion Dialogue 10

					},
					new SympathyText(
						new string[4] { "Oh, sorry!", "I didn't mean that.", "Sorry for offending you.", "Forgive me, please?" }
					),
					new ItemAcceptDialogue[1] {
						new ItemAcceptDialogue(
						new DialogueData[1] { 
							new DialogueData("You found it! Thanks for givine me this") 
						})
					},
					new ItemDeclineDialogue[1] {
						new ItemDeclineDialogue(
						new DialogueData[1] { 
							new DialogueData("Oh, I'm not looking for that") 
						})
					});
                # endregion Bart

                # region Maxine
                npcDataInfoList[7] = new NPCData(
					"Max",
                    "Maxine Paige",
                    Resources.AvatarDatabase.MaxineAvatar,
                    NPCNameID.Maxine,
                    "A very extroverted artist, who is already working in the industry and is not lactose intolerant. Oh, she likes music too!",
                    new Statistics(98, 95, 51, 62),
                    new ItemsNeeded[2] {
						new ItemsNeeded(ItemNameID.Headset), new ItemsNeeded(ItemNameID.Milk)
					},
                    new NPCDialogue[10] {

					# region Dialogue 1
						new NPCDialogue(
							new DialogueData[3] {
								new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Hi there! Ahahahahaha!"),
                                new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "You’re one of the lower batches right? Ahahahaha!"),
                                new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "I’m Maxine, don’t forget okay?", new DialogueButton[2] {
									new DialogueButton("You’re one odd and crazy character.", 1, ButtonType.Correct, new Statistics(10, 0), new DialogueData[2] {
										new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "I’ve been told! Hahahaha!"),
                                        new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "And I’m proud to be hahaha!")
									}),
									new DialogueButton("Okay?", 5, ButtonType.Wrong, KeypressLevel.Level1, new DialogueData[3] {
										new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "Geez, don’t be awkward!"),
                                        new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "I’m your senpai! Senpai!"),
                                        new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Lighten up, alright?")
									})
								})
							}
						),
					# endregion Dialogue 1

					# region Dialogue 2
						new NPCDialogue(
							new DialogueData[4] {
                                new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "We meet again!"),
                                new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Is it destiny? Hahaha just kidding!"),
                                new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "What are you doing wandering in the hallways anyway?"),
                                new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "Don’t you have class?" , new DialogueButton[2] {
									new DialogueButton("Nothing in particular!", 6, ButtonType.Wrong, KeypressLevel.Level1, new DialogueData[4] {
										new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Oooooh~ So defensive!"),
                                        new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "You're hiding something, aren't you? Aren't you?"),
                                        new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "I'll back off now!"),
                                        new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "You touchy touchy, defensive person you! Ahahaha!")
									}),
                                    new DialogueButton("I don't have any, I really am just wandering around!", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3] {
										new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "Aren't you bored?"),
                                        new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "You amaze me at the same time weird me out"),
                                        new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "BUT in a good way! Ahahahaha!")
									})
									
								})
							}
						),
					# endregion Dialogue 2

					# region Dialogue 3
						new NPCDialogue(
							new DialogueData[5] {
								new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Hey! If you have troubles with 3D, don’t hesitate to ask me!"),
                                new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "Uhh contacts, contacts…"),
                                new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Ah! Just contact me through facebook"),
                                new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "or maybe if you know some of my friends like Jenevieve, my love and Franz!"),
                                new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Then you can contact me through them! Ahahaha!", new DialogueButton[2] {
									new DialogueButton("Oh, thank you!", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5] {
										new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Not a problem, my dear! Ahahaha just kidding!"),
                                        new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Actually, it's also one of my excuses"),
                                        new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "so that I could be pm'ed by my real dear and love~"),
                                        new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Jenevieeeeveee~ hahahaha!"),
                                        new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "So contact me as often as possible through her, okay? ahahaha!")
									}),
									new DialogueButton("They sound familiar but, no.", 7, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[4] {
										new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "Why don't you know them?"),
                                        new DialogueData(Resources.AvatarDatabase.MaxineMadAvatar, "You've been studying here for how many years already"),
                                        new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "and you don't know them?"),
                                        new DialogueData(Resources.AvatarDatabase.MaxineSadAvatar, "Well, that sucks. Just contact me in facebook then.")
									})
								})

						}),
					# endregion Dialogue 3

					# region Dialogue 4
						new NPCDialogue(new DialogueData[3] {
                            new DialogueData(Resources.AvatarDatabase.MaxineSadAvatar, "I’m so getting in trouble!"),
                            new DialogueData(Resources.AvatarDatabase.MaxineMadAvatar, "Damn, I need to find that headset!"),
                            new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "Where did I put it? Did I lend it to someone?", new DialogueButton[3] {
                                new DialogueButton("You can ask the lost and found you know?", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[2] {
									new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Oh yeah! It might be there! Hahaha"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Thanks for the idea! I hope it's there.")
								}),
								new DialogueButton("You could always buy a new one.", 8, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[3] {
									new DialogueData(Resources.AvatarDatabase.MaxineMadAvatar, "I'm already working!"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineMadAvatar, "I don't want to waste anymore money!"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineSadAvatar, "Ugh, I'm deaaaaad.")
								}),
                                new DialogueButton("Just remember where you were last in with it?", 4, ButtonType.Correct, new DialogueData[3] {
									new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Ah! I'll look for Jenevieve and ask her~! Hahaha!"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Another excuse to go back where she is ahaha"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "Wait. I really should be more worried about the headset.")
								})
							})
						}),
					# endregion Dialogue 4

					# region Dialogue 5
						new NPCDialogue(new DialogueData[5] {
                            new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Hey! You joining the game jam?"),
                            new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Join the game jam!"),
                            new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "It’s going to be fun! There’s food! Ahahaha"),
                            new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "I know I’m joining, so you should too!"),
							new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Come on!" , new DialogueButton[3] {
								new DialogueButton("I already decided you before you asked.", 9, ButtonType.Correct, new Statistics(10, 0), new DialogueData[3] {
									new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Good!"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Then, I don't have to convince you some more."),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Just be sure to prepare! Hahaha I'll see you there!")
								}),
                                new DialogueButton("Are you bribing me with food?", 9, ButtonType.Wrong, KeypressLevel.Level3, new DialogueData[2] {
									new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "How would you even know that you won’t enjoy during the game jam?"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineMadAvatar, "Could you not assume things?")
								}),
                                new DialogueButton("Are you really this pushy and convincing?", 9, ButtonType.Correct, new Statistics(10, 0), new DialogueData[5] {
									new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Only to people that are not going! Hahaha!"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "Is it working? If it does,"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "I'll do this to everyone I see who has the look"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "of not wanting to participate. Hahaha!"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "I bet I can convince every people I see to join.")
								})
							})
						}),
					# endregion Dialogue 5

					# region Dialogue 6
						new NPCDialogue(new DialogueData[5] {
                            new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Since I'm already in the industry,"),
                            new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "don't you want to ask something about it? Hahaha"),
                            new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "I mean I've been talking to you for how many times now, "),
                            new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "and you still haven't asked me anything about it"),
                            new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "which is very surprising. Hahaha!", new DialogueButton[3]{
								new DialogueButton("Ah! I actually wanted to ask before....", 1, ButtonType.Correct, new Statistics(10, 0), new DialogueData[1] {
									new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Now that I'm here again, ask away! hahaha")
								}),
                                  new DialogueButton("I'm not confident to ask and know." + "I might get discouraged.", 6, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[5] {
									new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Don't be! Hahaha"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "I'm sure you can take on any challenges!"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "It's only difficult at the beginning because,"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "you have to adapt to a new set of culture and the likes."),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "But, once you get to used to it, it's just chill! .. well somehow hahaha!")
								}),
								new DialogueButton("Uh, is the industry out there difficult?", 6, ButtonType.Wrong, KeypressLevel.Level2, new DialogueData[7] {
									new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "I guess it depends on what you think is difficult."),
                                    new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "In my case, where I work, it's easy to get in and easy to get out."),
                                    new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "But the real challenge is the monthly test that we have to go through"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "where we should try to pass."),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "There are makeup tests, but, of course,"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "you'd want to have an excellent record."),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "to keep your job.")
								})
                              
							})
						}),
					# endregion Dialogue 6

					# region Dialogue 7
						new NPCDialogue(new DialogueData[2] {
							new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "I wonder what kind of people will be at the game jam this time!"),
                            new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Buuuut, I can pretty much guess it's the same old people. Hahaha!", new DialogueButton[3] {
								new DialogueButton("Whoever they are, I'm gonna beat them hands down.", 7, ButtonType.Wrong, KeypressLevel.Level3, new DialogueData[5] {
									new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "That's some confidence from you."),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Just remember."),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Do not let all that confidence get in your head! Hahaha"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "It might get bigger than it already is!"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineSadAvatar, "Seriously though, be careful on what you say, okay?")
								}),
                                new DialogueButton("The important thing here is to learn from this.", 7, ButtonType.Wrong, KeypressLevel.Level3, new DialogueData[2] {
									new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "How boring!"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineMadAvatar, "Where's your competitive spirit?!")
								}),
                                new DialogueButton("Hey, you never know some new people will participate.", 2, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4] {
									new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Expect the unexpected? Hahaha"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Quite so."),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Hmm~ it's going to be more interesting then if that's the case!"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Can't wait! Hahaha")
								})
							})
						}),
					# endregion Dialogue 7

					# region Dialogue 8
						new NPCDialogue(new DialogueData[2] {
							new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "I'm still thinking about whether to buy a drink or not."),
                            new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "I'd like some milk but, I'll eat soon anyways.", new DialogueButton[3] {
								new DialogueButton("Hmm, why don't you buy one and when you eat with everyone else,", 3, ButtonType.Correct, new Statistics(10, 0), new DialogueData[2]{
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "I can do that but I want something frizzy to drink, too! Hahaha!"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "I just can't decide!")
                                }),
								new DialogueButton("Just buy one and get over it.", 8, ButtonType.Wrong, KeypressLevel.Level4, new DialogueData[4]{
                                    new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "Are you in a hurry or something?"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "If you want, you can go ahead!"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineSadAvatar, "You don't have to wait for me to get to talk to me."),
                                    new DialogueData(Resources.AvatarDatabase.MaxineSadAvatar, "You might see me later anyways!")
                                }),
                                new DialogueButton("I'll eat ahead of you with the others." + "See you.", 8, ButtonType.Wrong, KeypressLevel.Level4, new DialogueData[3]{
                                    new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "Ooohh, so that's the way you want it"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineSadAvatar, "Well..."),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "I don't mind at all! Hahaha")
                                })
							})
						}),
					# endregion Dialogue 8

					# region Dialogue 9
						new NPCDialogue(new DialogueData[8] {
							new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Have you decided what type of game you might want to make on game jam?"),
                            new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "Pshh, theme doesn’t matter as long as you can incorporate your idea on it."),
                            new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "So… are you going 2D or 3D?"),
                            new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "It’s much more advisable to do 2D if it’s 3 days but,"),
                            new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "if you have an excellent 3D artist"),
                            new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "or if you’re an excellent 3D artist yourself..."),
                            new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "then, what the heck? Go for 3D!"),
                            new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Or you might want 2.5D? Hahaha!", new DialogueButton[3] {
                                new DialogueButton("Maybe a 2D or 2.5D!", 9, ButtonType.Wrong, KeypressLevel.Level5, new DialogueData[3]{
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Geez, decide only one! Hahaha!"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "You can't be indecisive during game jam okay?"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Every second counts!")
                                }),
								new DialogueButton("I’m not exactly sure yet.", 4, ButtonType.Correct, new Statistics(10, 0), new DialogueData[4]{
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Suit yourself!"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Just be ready during on the brainstorming event alright? Ahaha"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "After all, everyone has to pitch in their game ideas during that time!"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "I’m sure everyone will do fine!")
                                }),
								new DialogueButton("I want to do 3D! 3D's easy peasy~", 9, ButtonType.Wrong, KeypressLevel.Level5, new DialogueData[7]{
                                    new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "If you decide on that, just make sure you finish the game okay?"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "You can still rethink this though while you still have the time."),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Good luck! Hahaha"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "I mean if 3D's easy for you then"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "I'll look forward to the game that you'll create with it!"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "If you're having a hard time,"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "not my fault you thought 3D was easy~ hahaha!")
                                })
                                
							})
						}),
					# endregion Dialogue 9

					# region Dialogue 10

                        // level6 mini game pls
						new NPCDialogue(new DialogueData[4] {
							new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "Are you ready?"),
                            new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Come on! Ahaha Stop being nervous!"),
                            new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Game jam is there to let you enjoy creating games "),
                            new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "and getting along with other people!", new DialogueButton[3] {
								new DialogueButton("Alright! I'll do my best to not get nervous.", ButtonType.Correct, new Statistics(10, 0), new DialogueData[3]{
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "I can see the nervousness though hahaha!"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "But I guess that's better than pure nervousness though! Since..."),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "I can also see you're as excited as me! Hahaha!")
                                }),
                                new DialogueButton("I know that and I AM controlling my nerves.", ButtonType.Correct, new Statistics(10, 0), new DialogueData[5]{
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Well, control them more! Hahaha"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Until all I see is excitement and confidence!"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "If you have something to be proud off that would help you in game jam,"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "Flaunt it or at least show them that you're proud alright? hahaha"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineHappyAvatar, "If you can do that, then you're ready!")
                                }),
								new DialogueButton("I can't help it okay?!", ButtonType.Wrong, KeypressLevel.Level5, new DialogueData[2]{
                                    new DialogueData(Resources.AvatarDatabase.MaxineWeirdAvatar, "Hey hey~ Chill!"),
                                    new DialogueData(Resources.AvatarDatabase.MaxineSadAvatar, "Just encouraging you to not let nervousness get to you.")
                                })
							})
						}),
					# endregion Dialogue 10

					},
					new SympathyText(
						new string[4] { "Really Sorry", "Forgive me.", "It's not really like that!", "I apologize." }
					),
						new ItemAcceptDialogue[1] {
						new ItemAcceptDialogue(
						new DialogueData[1] { 
							new DialogueData("Hahaha! How did you know I was looking for this") 
						})
					},
					new ItemDeclineDialogue[1] {
						new ItemDeclineDialogue(
						new DialogueData[1] { 
							new DialogueData("Sorry I don't need this") 
						})
					});
                # endregion Maxine

				// Used for shuffling dialogue lines
				sortingDataList = new List<NPCData>(npcDataInfoList);
			}

			// --- DO NOT EDIT BELOW THIS LINE --- //

			public static void AddNpcStatistics(NPCNameID nameID, Statistics addedStat) {
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

			public static void AddNpcItemsHave(NPCNameID nameID, ItemNameID itemId) {
				ItemData[] items = GetNpc(nameID).NpcItemsHave;
				for (int i = 0; i < items.Length; i++) {
					if (items[i] == null) {
						items[i] = ItemDatabase.GetItem(itemId);
						Debug.Log("[NPC ITEMS] " + items[i].ItemName + " has been added to " + nameID.ToString() + " items have.");
						break;
					}
				}
				GetNpc(nameID).NpcItemsHave = items;
			}

			public static void GiveNpcItem(NPCNameID nameID, ItemNameID itemID, int inventoryIndx) {
				NPCManager npcManager = NPCManager.current;
				UserInterface userInterface = UserInterface.current;
				GameManager gameManager = GameManager.current;
				PlayerInformation playerInformation = gameManager.BasePlayerData.PlayerInformation;

				ItemsNeeded itemRecieved = GetNpcItemReceived(nameID, itemID);
				NPCInformation npcInformation = npcManager.GetNpcInformation(nameID);

				userInterface.GivingItem = false;
				playerInformation.RemoveInventoryItem(itemID, inventoryIndx);

				if (itemRecieved.ItemID != ItemNameID.None && !itemRecieved.ItemRecieved) {
					itemRecieved.ItemRecieved = true;
					AddNpcItemsHave(nameID, itemID);

					if (itemRecieved.AddedStatistics != Statistics.zero) {
						AddNpcStatistics(nameID, itemRecieved.AddedStatistics);
						Debug.Log("[NPC STATISTICS] " + npcInformation.BaseNpcData.NpcName + " has Incresed Stats " + itemRecieved.AddedStatistics.ToString());
					}

					npcInformation.RunEmoticon(EmoticonNameID.Happy);
					DialogueManager.current.RunReplyDialogue(nameID, ReplyType.Accept, ItemDatabase.GetItem(itemID).ItemName);
				}
				else {
					ItemData item = ItemDatabase.GetItem(itemID);
					Statistics beforeStat = npcInformation.BaseNpcData.NpcStatistics;
					Statistics afterStat = beforeStat + item.ItemDebuffStat;
					AddNpcStatistics(nameID, item.ItemDebuffStat);

					Debug.Log("[NPC ITEM NEEDED] " + npcInformation.BaseNpcData.NpcName + " doesn't require " + item.ItemName + ".");
					Debug.Log("[NPC ITEM DEBUFF] " + npcInformation.BaseNpcData.NpcName + " has recieved a debuff of "
						+ item.ItemDebuffStat + " because of wrong item. " + beforeStat.ToString() + " -> " + afterStat.ToString());

					npcInformation.RunEmoticon(EmoticonNameID.Sad);
					DialogueManager.current.RunReplyDialogue(nameID, ReplyType.Decline, ItemDatabase.GetItem(itemID).ItemName);
				}
			}

			public static NPCData GetNpc(NPCNameID nameID) {
				NPCData npcData = new NPCData();
				if (nameID != NPCNameID.None) {
					for (int i = 0; i < npcDataInfoList.Length; i++) {
						if (npcDataInfoList[i] != null && npcDataInfoList[i].NpcNameID == nameID) {
							npcData = npcDataInfoList[i];
						}
					}
				}
				else {
					Debug.Log("NameID is None. NPCData -> GetNpc");
				}
				return npcData;
			}

			public static ItemsNeeded GetNpcItemNeeded(NPCNameID nameID, ItemNameID itemID) {
				NPCData tmpNpcData = GetNpc(nameID);
				ItemsNeeded itemNeeded = new ItemsNeeded();
				for (int i = 0; i < tmpNpcData.NpcItemsNeeded.Length; i++) {
					if (tmpNpcData.NpcItemsNeeded[i].ItemID == itemID) {
						itemNeeded = tmpNpcData.NpcItemsNeeded[i];
					}
				}
				return itemNeeded;
			}

			public static ItemsNeeded GetNpcItemReceived(NPCNameID nameID, ItemNameID itemID) {
				NPCData tmpNpcData = GetNpc(nameID);
				ItemsNeeded itemNeeded = new ItemsNeeded();
				for (int i = 0; i < tmpNpcData.NpcItemsNeeded.Length; i++) {
					if (tmpNpcData.NpcItemsNeeded[i].ItemID == itemID && !tmpNpcData.NpcItemsNeeded[i].ItemRecieved) {
						itemNeeded = tmpNpcData.NpcItemsNeeded[i];
						break;
					}
				}
				return itemNeeded;
			}

			public static NPCDialogue GetNpcDialogue(int indx, NPCNameID nameID) {
				NPCDialogue npcDialogue = new NPCDialogue();
				if (nameID != NPCNameID.None) {
					npcDialogue = GetNpcDialogueList(nameID)[indx];
				}
				else {
					Debug.Log("NameID is None. NPCData -> GetNPCDialogue");
				}
				return npcDialogue;
			}

			public static NPCDialogue[] GetNpcDialogueList(NPCNameID nameID) {
				if (nameID != NPCNameID.None) {
					return GetNpc(nameID).NpcDialogue;
				}
				Debug.Log("NameID is None. NPCData -> GetNpcDialogueList");
				return null;
			}

			public static DialogueData[] GetNpcSelectionDialogueData(int indx, NPCNameID nameID) {
				if (nameID != NPCNameID.None) {
					return GetNpcDialogue(indx, nameID).DialogueData;
				}
				Debug.Log("NameID is None. NPCData -> GetNPCSelection");
				return null;
			}

			public static DialogueData[] GetNpcContinousDialogueData(NPCNameID nameID) {
				if (nameID != NPCNameID.None) {
					NPCData tmpSortingData = GetNpc(nameID);
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

			public static DialogueData[] GetNpcRandomDialogueData(NPCNameID nameID) {
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

			public static DialogueData[] GetNpcRandomAcceptedText(NPCNameID nameID) {
				if (nameID != NPCNameID.None) {
					NPCData tmpSortingData = sortingDataList[(int)nameID - 1];
					if (tmpSortingData.NpcItemAcceptDialogue != null) {
						List<ItemAcceptDialogue> acceptedTextList = new List<ItemAcceptDialogue>(tmpSortingData.NpcItemAcceptDialogue);
						DialogueData[] result = new DialogueData[acceptedTextList.Count];
						result = acceptedTextList[0].DialogueData;

						if (acceptedTextList.Count > 1) {
							do {
								acceptedTextList.Sort((x, y) => Random.value < 0.5f ? -1 : 1);
							}
							while (result == acceptedTextList[0].DialogueData);
						}

						tmpSortingData.NpcItemAcceptDialogue = acceptedTextList.ToArray();
						result = acceptedTextList[0].DialogueData;
						return result;
					}
					else 
						return null;
				}
				Debug.Log("NameID is None. NPCData -> GetNPCRandomAcceptedText");
				return null;
			}

			public static DialogueData[] GetNpcRandomDeclineText(NPCNameID nameID) {
				if (nameID != NPCNameID.None) {
					NPCData tmpSortingData = sortingDataList[(int)nameID - 1];
					if (tmpSortingData.NpcItemDeclineDialogue != null) {
						List<ItemDeclineDialogue> declinedTextList = new List<ItemDeclineDialogue>(tmpSortingData.NpcItemDeclineDialogue);
						DialogueData[] result = new DialogueData[declinedTextList.Count];
						result = declinedTextList[0].DialogueData;

						if (declinedTextList.Count > 1) {
							do {
								declinedTextList.Sort((x, y) => Random.value < 0.5f ? -1 : 1);
							}
							while (result == declinedTextList[0].DialogueData);
						}

						tmpSortingData.NpcItemDeclineDialogue = declinedTextList.ToArray();
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