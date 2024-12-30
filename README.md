![Fentanyl Reactor](https://github.com/user-attachments/assets/4a693450-9907-457e-9643-cdb310d66298)
<br><br><br>
[![downloads](https://img.shields.io/github/downloads/FentanylReactorGER/FentanylReactorPlugin/total?style=for-the-badge&logo=icloud&color=%233A6D8C)](https://github.com/FentanylReactorGER/FentanylReactorPlugin/releases/latest)
![Latest](https://img.shields.io/github/v/release/FentanylReactorGER/FentanylReactorPlugin?style=for-the-badge&label=Latest%20Release&color=%23D91656)

# Fentanyl Reactor for EXILED

### Minimum Exiled Version: [9.1.1](https://github.com/ExMod-Team/EXILED/releases/latest)
## Features:
- Adding A Custom Schematic
- Adding Custom Sounds
- Adding Custom Items
- Server Specific Setting
- Admin Commands
- Autonuke / Reactor Meltdown
- End Game Anit Camping Feature

# How to install?

- Just Download the Fentanyl_ReactorUpdate.dll and put it into EXILED/Plugins.
- Download the Dependencies.
- Restart your Server so the Schematics and Sounds will be Downloaded Automatically.

# Dependencies
- AudioPlayer by [@Killers0992](https://github.com/Killers0992) download here: [AudioPlayer](https://github.com/Killers0992/AudioPlayer/releases/latest)
- MapEditorReborn by [@Michal78900](https://github.com/Michal78900) download here: [MapEditorReborn](https://github.com/Michal78900/MapEditorReborn/releases/latest)
- HintServiceMeow by [@MeowServer](https://github.com/MeowServer) download here: [HintServiceMeow](https://github.com/MeowServer/HintServiceMeow/releases/latest)

### Additional:
- Advanced MER Tools by [@MujisongPlay](https://github.com/MujisongPlay) download here: [AMERT](https://github.com/MujisongPlay/AdvancedMERtools/releases/tag/Publish)
- Fentanyl Reactor Schematic incase Auto Updater Fails [Schematic](https://github.com/FentanylReactorGER/FentanylReactorSchematic/releases/latest)
- Demon Core Schematic incase Auto Updater Fails [Schematic](https://github.com/FentanylReactorGER/DemonCoreSchematic/releases/latest)
- Fentanyl Reactor OGG Sound incase Auto Updater Fails [OGG](https://github.com/FentanylReactorGER/FentanylAudio/releases/latest)
- Fentanyl Reactor Radiaton OGG Sound incase Auto Updater Fails [OGG](https://github.com/FentanylReactorGER/FentReactorDemonCoreOgg/releases/latest)

# Credits:
- Thanks [@Vretu-Dev](https://github.com/Vretu-Dev) for using the [UsefulHints](https://github.com/Vretu-Dev/UsefulHints/) idea for Auto Updating, and using your Readme as Example.<br> 
- Thanks [@MujisongPlay](https://github.com/MujisongPlay) for using the [Site76](https://github.com/MujisongPlay/ScpSite76Plugin) Elevator Schematic and Refill Panel.<br>
- Thanks [@Trevlouw](https://github.com/Trevlouw) for creating the Custom Items for me.<br>

## Config:

```yaml
# Should the plugin be enabled
is_enabled: true
# Should the plugin display a debug message
debug: false
# Should the plugin Check for Uopdates / Set this false if you want Custom Schematics / Sounds!
update: true
# Should the plugin create a backup
backup: false
# Should the plugin replace a room
replace_room: true
# Schematic name to replace
schematic_name: 'FentanylReactor'
# Room name to replace
room_type: HczTestRoom
kill_area_expansion_rate: 1
kill_area_effect_interval: 2
effect_one: Poisoned
effect_two: Burned
effect_three: Deafened
effect_duration: 1
kill_area_damage: 2
check_interval: 0.5
# Server Specific Settings
server_specific_setting_hold_time: 3
server_specific_setting_id: 511
server_specific_setting_id_fuel: 512
server_specific_setting_id_start: 513
# Should the plugin have a Hint whenever a player enters the Fentanyl Reactor?
enter_hint: true
# Should the plugin have a meltdown (This doesn't affect the Admin Command)
meltdown: true
# Meltdown Minimum time after round starts
meltdown_zeit_min_start_runde: 900
# Meltdown Maximum after round starts
meltdown_zeit_max_start_runde: 1500
# Minimum time to start a meltdown (subtracted from round start time)
meltdown_zeit_start: 10
# Maximum time to start a meltdown (subtracted from C.A.S.S.I.E time)
meltdown_zeit_end: 120
# Maximum time to start a meltdown (subtracted from C.A.S.S.I.E time)
use_cassie_instead_of_audio: false
# Meltdown Color
meltdown_color:
  r: 0.15
  g: 0
  b: 0.4
  a: 1
# Meltdown Demon Core Color
demon_core_color_melt:
  r: 0.2235294
  g: 1
  b: 0.07843138
  a: 1
# Demon Core Color
demon_core_color:
  r: 0.5
  g: 0.5
  b: 0.5
  a: 0.5
# Demon Core Schematic Name
demon_core_scheme_name: 'demon_core'
# Demon Core Position
demon_core_pos:
  x: 27
  y: 990.48
  z: -24.44
# Demon Core Rotation
demon_core_rot:
  x: 0
  y: 90
  z: 0
# Demon Core Scale
demon_core_scale:
  x: 1
  y: 1
  z: 1
# Demon Core Cooldown
demon_core_cooldown: 120
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
# Fentanyl Reactor Fentanyl stage 1 button name / Don't Touch this if you aren't a Developer!
button_stage1_name: 'Stage1'
# Fentanyl Reactor Fentanyl stage 2 button name / Don't Touch this if you aren't a Developer!
button_stage2_name: 'Stage2'
# Fentanyl Reactor Fentanyl stage 3 button name / Don't Touch this if you aren't a Developer!
button_stage3_name: 'Stage3'
# Fentanyl Reactor Fentanyl stage 3 button name / Don't Touch this if you aren't a Developer!
button_deom_core_name: 'Pickup_DemonCore'
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
t1_i_d: 88
t2_i_d: 89
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
# Command Name to Cancel the meltdown of the Fentanyl Reactor (Admins)
meltdown_cancel_command_name: 'ReactorCancelMeltdown'
# Command Name to Cancel the meltdown of the Fentanyl Reactor (Admins)
teleport_fentanyl: 'FentTP'
# Demon Core Death Reason
killarea_death_reason: 'Radiation'
# Demon Core Cooldown Hint
demon_core_cooldown_hint: 'The Demon Core remains in Cooldown for {DemonCoreStartCooldown} Seconds.'
# Demon Core Already Open Hint
demon_core_alr_open_hint: 'The Demon Core is already unlocked.'
# Demon Core Open Hint
demon_core_open_hint: 'The Demon Core is now unlocked.'
# Demon Core Ready to Open Hint
demon_core_ready_to_open_hint: 'The Demon Core is now able to be unlocked.'
# Command Name to Cancel the meltdown of the Fentanyl Reactor (Admins)
teleport_fentanyl_no_prem: 'You don''t have the Premission to use this Button!'
# Player not in Fentanyl Reactor Hint
fentanyl_reactor_s_s_fuel: 'You not inside the Fentanyl Reactor Basement buidling!'
# Fentanyl Reactor Teleport Premission
teleport_fentanyl_premission: 'FentTP'
# Server Specific Settings
s_s_s_sheader_player: 'Fentanyl Reaktor'
s_s_s_s_label_tp: 'Fentanyl Reactor Teleport'
s_s_s_s_desc_tp: 'Uses the Fentanyl Reactor Teleport'
s_s_s_s_tp_button: 'TP'
s_s_s_s_label_fuel: 'Fentanyl Reactor Fuel'
s_s_s_s_desc_fuel: 'Uses the Fentanyl Reactor Refueling'
s_s_s_s_fuel_button: 'Refill'
s_s_s_s_start_name: 'Fentanyl Reactor Start'
s_s_s_sl_stage1: 'Stage-1'
s_s_s_sl_stage2: 'Stage-2'
s_s_s_sl_stage3: 'Stage-3'
s_s_s_start_not_in_reactor: 'You are not Inside the Fentanyl Reactor to use this!'
s_s_s_s_round_not_started: 'Round is not Started which is needed for this to work!'
s_s_s_s_player_is_s_c_p: 'You are SCP, SCPs cannot use this!'
s_s_s_s_start_desc: 'Starts the Fentanyl Reactor on a given Stage.'
# Enter the Schematic hint, use {PlayerName} for the Players Name that enters the Reactor. (If Config is enabled)
enter_fentanyl_reactor: |-
  Welcome inside the Fentanyl Reactor! 
   {PlayerName}
# No Adrenaline Hint
no_adrenaline_hint: 'You have no adrenaline!'
# Meltdown CASSIE
fentanyl_reactor_meltdown_cassie: 'pitch_0,20 .G4 . .G4 . pitch_0,95 The Reactor is overheating pitch_0,20 .G4 . .G4 . pitch_0,82 evacuate immediately pitch_0,20 .G4 . .G4 . jam_017_15 .G4'
# Meltdown CASSIE Translation
fentanyl_reactor_meltdown_cassie_trans: 'The reactor is overheating! Evacuate immediately..'
# Fentanyl Reactor Refuel Hint
reactor_fueled: 'The Fentanyl Reactor has been refueled!'
# Fentanyl Reactor already refueled hint
reactor_already_fueled_hint: 'The Fentanyl Reactor is already refueled!'
# Fentanyl Reactor not refueled hint
reactor_not_fueled_hint: 'The Fentanyl Reactor is not refueled!'
# Fentanyl Reactor Starting Hint
reactor_starting_hint: 'Fentanyl Reactor is starting...'
# Fentanyl Reactor Success Hint Stage 1
reactor_success_hint_stage_one: 'Fentanyl Stage One is being generated!'
# Fentanyl Reactor Success Hint Stage 2
reactor_success_hint_stage_two: 'Fentanyl Stage Two is being generated!'
# Fentanyl Reactor Success Hint Stage 3
reactor_success_hint_stage_three: 'Fentanyl Stage Three is being generated!'
# Fentanyl Reactor Cooldown Hint, use {Cooldown} as Cooldown Variable
reactor_cooldown: 'The Fentanyl Reactor has a cooldown of: {Cooldown} seconds'
# Fentanyl Reactor could not produce anything hint
reactor_failure_hint: 'The Fentanyl Reactor could not produce anything!'
# Fentanyl Stage 1 Name
t1_name: 'Fentanyl Stage 1'
# Fentanyl Stage 1 Description
t1_description: 'Impure Fentanyl Stage 1!'
# Fentanyl Stage 2 Name
t2_name: 'Fentanyl Stage 2'
# Fentanyl Stage 2 Description
t2_description: 'Normal Fentanyl Stage 2!'
# Fentanyl Stage 3 Name
t3_name: 'Fentanyl Stage 3'
# Fentanyl Stage 3 Description
t3_description: 'Purest Fentanyl Stage 3!'
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
