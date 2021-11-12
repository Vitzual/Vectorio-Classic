I'm still working on streamlining the stats process, but as it stands right now, this is how it works...

1. The ScriptableManager script loads all SO's from the Resources folder
2. As buildings are loaded in, they're passed to the Stats class for setup
3. In the setup process, the building gets put into a dictionary with the value being a Stat object
4. The stat object then holds all the required stats for that scriptable (base, research, and engineering)
5. Everytime a stat gets updated, it will loop through all active instances of that building and update it 