![Fentanyl Reactor](https://github.com/user-attachments/assets/4a693450-9907-457e-9643-cdb310d66298)
<br><br><br>
[![downloads](https://img.shields.io/github/downloads/FentanylReactorGER/FentanylReactorPlugin/total?style=for-the-badge&logo=icloud&color=%233A6D8C)](https://github.com/FentanylReactorGER/FentanylReactorPlugin/releases/latest)
![Latest](https://img.shields.io/github/v/release/FentanylReactorGER/FentanylReactorPlugin?style=for-the-badge&label=Latest%20Release&color=%23D91656)

# Fentanyl Reactor for EXILED

### Minimum Exiled Version: [9.0.0](https://github.com/ExMod-Team/EXILED/releases/latest)
## Features:
- Adding A Custom Schematic
- Adding Custom Sounds
- Adding Custom Items
- Server Specific Setting
- Admin Commands
- Autonuke / Reactor Meltdown

# How to install?

- Just Download the Fentanyl_ReactorUpdate.dll and put it into EXILED/Plugins.
- Download the Dependencies.
- Restart your Server so the Schematics and Sounds will be Downloaded Automatically.

# Dependencies
- AudioPlayer by [@Killers0992](https://github.com/Killers0992) download here: [AudipPlayer](https://github.com/Killers0992/AudioPlayer/releases/latest)
- MapEditor Reborn by [@Michal78900](https://github.com/Michal78900) download here: [MapEditorReborn](https://github.com/Michal78900/MapEditorReborn/releases/latest)

### Additional:
- Advanced MER Tools by [@MujisongPlay](https://github.com/MujisongPlay) download here: [AMERT](https://github.com/MujisongPlay/AdvancedMERtools/releases/tag/Publish)
- Fentanyl Reactor Schematic incase Auto Updater Fails [Schematic](https://github.com/FentanylReactorGER/FentanylReactorSchematic/releases/latest)
- Fentanyl Reactor OGG Sound incase Auto Updater Fails [OGG](https://github.com/FentanylReactorGER/FentanylAudio/releases/latest)

# Credits:
- Thanks [@Vretu-Dev](https://github.com/Vretu-Dev) for using the [UsefulHints](https://github.com/Vretu-Dev/UsefulHints/) idea for Auto Updating, and using your Readme as Example.<br> 
- Thanks [@MujisongPlay](https://github.com/MujisongPlay) for using the [Site76](https://github.com/MujisongPlay/ScpSite76Plugin) Elevator Schematic and Refill Panel.<br>
- Thanks [@Trevlouw](https://github.com/Trevlouw) for creating the Custom Items for me.<br>

## Config:

```yaml
# Should the plugin be enabled
is_enabled: true
# Should the plugin display a debug message
debug: true
# Should the plugin create a backup
backup: false
# Should the plugin replace a room
replace_room: true
# Schematic name to replace
schematic_name: 'FentanylReactor'
# Room name to replace
room_type: HczTestRoom
# Meltdown time after round starts
meltdown_zeit_start_runde: 1500
# Minimum time to start a meltdown (subtracted from round start time)
meltdown_zeit_start: 10
# Maximum time to start a meltdown (subtracted from round start time)
meltdown_zeit_end: 120
# Fentanyl Reactor Stage 1 success chance
level1_chance: 0.75
# Fentanyl Reactor Stage 2 success chance
level2_chance: 0.550000012
# Fentanyl Reactor Stage 3 success chance
level3_chance: 0.25
# Fentanyl Reactor audio volume
fentanyl_reactor_audio_volume: 1
# Fentanyl Reactor audio Distance Min
fentanyl_reactor_audio_min: 10
# Fentanyl Reactor audio Distance Max
fentanyl_reactor_audio_max: 20
# Fentanyl Reactor Fentanyl stage 1 button name
button_stage1_name: 'Stage1'
# Fentanyl Reactor Fentanyl stage 2 button name
button_stage2_name: 'Stage2'
# Fentanyl Reactor Fentanyl stage 3 button name
button_stage3_name: 'Stage3'
# Fentanyl Reactor refill button name
button_refill_name: 'Refill'
# Fentanyl Reactor global hint duration
global_hint_duration: 5
# Fentanyl Reactor command cooldown
command_cooldown: 60
# Fentanyl Reactor wait time until product
reactor_wait_time: 10
# The zombification chance for each Fentanyl stage
t1_zombie_chance: 0.550000012
t2_zombie_chance: 0.349999994
t3_zombie_chance: 0.100000001
# The change in intensity for each effect at each Fentanyl stage
t1_intensity: 1
t2_intensity: 1
t3_intensity: 1
# The number of times each Fentanyl stage increases the intensity of an effect (by its respective intensity)
t1_looping: 1
t2_looping: 3
t3_looping: 10
# The delay between the usage and the effects of Fentanyl
t1_delay: 2.5
t2_delay: 2.5
t3_delay: 2.5
# The change in movement speed due to Fentanyl
t1_movement_speed: 5
t2_movement_speed: 25
t3_movement_speed: 50
# The minimum duration of Fentanyl effects
t1_duration_lower: 5
t2_duration_lower: 10
t3_duration_lower: 15
# The maximum duration of Fentanyl effects
t1_duration_upper: 10
t2_duration_upper: 20
t3_duration_upper: 30
# The custom item ID for the Fentanyl item
t1_i_d: 1
t2_i_d: 14
t3_i_d: 90
# The weight of each Fentanyl item
t1_weight: 1
t2_weight: 1
t3_weight: 1
```

## Translation:

```yaml
# Command Name to use the Fentanyl Reactor (Admins)
command_name: 'FentanylReactorCore'
# Command Name to refuel the Fentanyl Reactor (Admins)
fuel_command_name: 'FentanylReactorFuel'
# Command Name to meltdown the Fentanyl Reactor (Admins)
meltdown_command_name: 'ForceReactorMeltdown'
# No Adrenaline Hint
no_adrenaline_hint: 'Du hast kein Adrenalin!'
# Meltdown CASSIE
fentanyl_reactor_meltdown_cassie: 'pitch_0,20 .G4 . .G4 . pitch_0,95 The Reactor is overheating pitch_0,20 .G4 . .G4 . pitch_0,82 evacuate immediately pitch_0,20 .G4 . .G4 . jam_017_15 .G4'
# Meltdown CASSIE Translation
fentanyl_reactor_meltdown_cassie_trans: 'Der Reaktor überhitzt! Sofort EVAKUIEREN..'
# Fentanyl Reactor Refuel Hint
reactor_fueled: 'Der Fentanyl Reaktor wurde aufgefüllt!'
# Fentanyl Reactor already refueled hint
reactor_already_fueled_hint: 'Der Fentanyl Reaktor ist bereits aufgefüllt!'
# Fentanyl Reactor not refueled hint
reactor_not_fueled_hint: 'Der Fentanyl Reaktor ist nicht aufgefüllt!'
# Fentanyl Reactor Starting Hint
reactor_starting_hint: 'Fentanyl Reaktor startet...'
# Fentanyl Reactor Success Hint Stage 1
reactor_success_hint_stage_one: 'Fentanyl Stufe Eins wird generiert!'
# Fentanyl Reactor Success Hint Stage 2
reactor_success_hint_stage_two: 'Fentanyl Stufe Zwei wird generiert!'
# Fentanyl Reactor Success Hint Stage 3
reactor_success_hint_stage_three: 'Fentanyl Stufe Drei wird generiert!'
# Fentanyl Reactor Cooldown Hint
reactor_cooldown: 'Der Fentanyl Reaktor hat eine Abklingzeit von:'
# Fentanyl Reactor could not produce anything hint
reactor_failure_hint: 'Der Fentanyl Reaktor konnte nichts produzieren!'
# Fentanyl Stage 1 Name
t1_name: 'Fentanyl Stufe 1'
# Fentanyl Stage 1 Description
t1_description: 'Unreines Fentanyl Stufe 1!'
# Fentanyl Stage 2 Name
t2_name: 'Fentanyl Stufe 2'
# Fentanyl Stage 2 Description
t2_description: 'Normales Fentanyl Stufe 2!'
# Fentanyl Stage 3 Name
t3_name: 'Fentanyl Stufe 3'
# Fentanyl Stage 3 Description
t3_description: 'Reinstes Fentanyl Stufe 3!'
```
## Showcase:

### Schematic:
<p align="center">
    <img src="https://github.com/user-attachments/assets/28a2aa24-e982-432e-88f0-d10b4201bfc1">
</p>

### Use:
https://github.com/user-attachments/assets/cd4e5730-d218-4e2a-9f29-c11b212bb134

### Refill:
https://github.com/user-attachments/assets/dcfa8762-3290-4861-99a1-1cdcc0e54638
