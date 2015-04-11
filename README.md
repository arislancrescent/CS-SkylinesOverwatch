# Skylines Overwatch  
Steam Workshop: [[ARIS] Skylines Overwatch](http://steamcommunity.com/sharedfiles/filedetails/?id=421028969)

Efficient active ID monitoring framework to monitor and categorize buildings, vehicles, citizens, and animals in the city. Pause the game to see a summary of everything currently tracked in the debug window. 

Check out the new [FAQ](https://github.com/arislancrescent/CS-SkylinesOverwatch/wiki/FAQ)!

## What is this?

This is a monitoring framework other mods can attach to to get the active IDs for the categories they need, and saving mod developers from having to separate the active IDs from inactive ones themselves.

## Why is this needed?

Cities: Skylines' code does not automatically provide direct access to active IDs. For example, buildings have a maximum set of 32,768 possible IDs. When creating a new building, the game randomly selects an unused ID out of the full set. But without a record of all the active IDs, a modder seeking to check all created buildings would have to loop through the entire 32,768 possible IDs just to find the handful that are actually active. This creates several issues:

1. Without proper attention, a mod performing such checks can be very inefficient and lead to significant FPS drops. The situation compounds when multiple mods are performing the same checks.

2. Even with optimization, as long as a mod doesn't match the game perfectly, it can have delays in detecting changes. For example, a mod that detects the spawning of seagulls can have new seagulls flying around for several seconds before even realizing they are there. For many applications, this kind of delay in reaction harms player experience greatly.

Skylines Overwatch solves both of these issues and frees up modders to focus on what they truly want to accomplish.

## Why Skylines Overwatch?

1. It is **_efficient_**. Skylines Overwatch is designed from the ground up to be efficient. It does not "poll" the environment to get what's out there. It checks the exact same entries as the game during each frame. There are no wasted CPU cycles. 

2. It is **_responsive_**. Skylines Overwatch mirrors the game environment: it does not know any more or any less than the game itself. Skylines Overwatch becomes aware of something as soon as the game does. There is no delay that usually accompanies typical monitoring systems. 

3. It is **_simple_**. Which would you prefer? Polling tens of thousands of entries every few seconds just to find the handful of active ones, or let Skylines Overwatch tell you which ones are active and which have been just updated? 

4. It is **_safe_**. To accomplish what they want to do, some mods hack into private variables, others completely replace game codes. While necessary in certain situations, too many mods out there are doing this simply to avoid monitoring the game environment themselves, probably due to the significant performance hit that usually comes with it. This is the #1 reason why players are getting corrupt game files everywhere. Skylines Overwatch does not and will not change anything in the game. 

5. It is **_lightweight_**. Skylines Overwatch has no other purposes: it watches over the city and that's it. This allows it to be coded in a very specialized way to achieve the best efficiency possibly. Furthermore, Skylines Overwatch uses the singleton design pattern, which means it does not iterate over the same item more than once, regardless of how many mods might be using it. Imagine 10 mods all checking on the vehicles in the game, each of them doing it their own way. That's potentially 10 checks per entry 60 times per second for each of the tens of thousands of entries. Skylines Overwatch allows ALL attached mods to achieve at least the same, if not more, by doing 1 check per entry at most 4 times per second for only a fraction of the total possible entries. 

6. It is **_modular_**. Skylines Overwatch allows developers a significant level of flexibility in deciding what goes into their mods. Often, developers cram in a long list of functionalities into one mod for efficiency reasons. Skylines Overwatch takes care of a lot of that concern. This means that, as a developer, you can create single-task mods that do only one thing, but can do it very well; and as a player, you can pick and choose which features you want, without having to download a truck load of others that you don't want but bundled together. Modularity drives specialization; specialization enables the next level in quality. This is the core purpose of Skylines Overwatch. 

## How to use Skylines Overwatch?

Check out the [API documentation](https://github.com/arislancrescent/CS-SkylinesOverwatch/wiki)
