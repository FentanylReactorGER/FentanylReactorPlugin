![UsefulHints]([https://github.com/user-attachments/assets/a01fc940-f540-4c8b-8caf-65848a22335d](https://github.com/user-attachments/assets/f5aa3e98-d73c-470d-a134-5e9d9))<br><br><br>
[![downloads](https://img.shields.io/github/downloads/Vretu-Dev/UsefulHints/total?style=for-the-badge&logo=icloud&color=%233A6D8C)](https://github.com/Vretu-Dev/UsefulHints/releases/latest)
![Latest](https://img.shields.io/github/v/release/Vretu-Dev/UsefulHints?style=for-the-badge&label=Latest%20Release&color=%23D91656)

# Fentanyl Reactor for EXILED

### Minimum Exiled Version: 8.14.0
## Features:
- Adding A Custom Schematic
- Adding Custom Sounds
- Adding Custom Items

### Credits:
- Thanks [@Killers0992](https://github.com/Killers0992) for using the [Audio Player](https://github.com/Killers0992/AudioPlayer) .<br>
- Thanks [@Vretu-Dev](https://github.com/Vretu-Dev) for using the [UsefulHints](https://github.com/Vretu-Dev/UsefulHints/) idea for Auto Updating, and using your Readme as Example.<br>

## Config:

```yaml
UH:
  is_enabled: true
  debug: false
  # Auto Translations:
  translations: true
  # Available Languages: pl, en, de, fr, cs, sk, es, it, pt, ru, tr, zh
  language: 'en'
  translations_path: '/home/container/.config/EXILED/Configs/UsefulHints/Translations'
  # Auto Update:
  auto_update: true
  enable_logging: true
  enable_backup: false
  plugin_path: '/home/container/.config/EXILED/Plugins/UsefulHints.dll'
  # Hint Settings:
  enable_hints: true
  scp096_look_message: 'You looked at SCP-096!'
  scp268_time_left_message: 'Remaining: {0}s'
  scp2176_time_left_message: 'Remaining: {0}s'
  scp1576_time_left_message: 'Remaining: {0}s'
  grenade_damage_hint: '{0} Damage'
  jailbird_use_message: 'Remaining charges: {0}'
  scp207_hint_message: 'You have {0} doses of SCP-207'
  anti_scp207_hint_message: 'You have {0} doses of Anti SCP-207'
  show_hint_on_equip_item: false
  # Item Warnings:
  enable_warnings: true
  scp207_warning: '<color=yellow>⚠</color> You are already affected by <color=#A60C0E>SCP-207</color>'
  anti_scp207_warning: '<color=yellow>⚠</color> You are already affected by <color=#2969AD>Anti SCP-207</color>'
  scp1853_warning: '<color=yellow>⚠</color> You are already affected by <color=#1CAA21>SCP-1853</color>'
  # Friendly Fire Warning:
  enable_ff_warning: true
  friendly_fire_warning: '<size=27><color=yellow>⚠ Do not hurt your teammate</color></size>'
  damage_taken_warning: '<size=27><color=red>{0}</color> <color=yellow>(teammate) hit you</color></size>'
  class_d_are_teammates: true
  enable_cuffed_warning: false
  cuffed_attacker_warning: '<size=27><color=yellow>⚠ Player is cuffed</color></size>'
  cuffed_player_warning: '<size=27><color=red>{0}</color> <color=yellow>hit you when you were cuffed</color></size>'
  # Kill Counter:
  enable_kill_counter: true
  kill_count_message: '{0} kills'
  # Round Summary:
  enable_round_summary: true
  round_summary_message_duration: 10
  human_kill_message: '<size=27><color=#70EE9C>{0}</color> had the most kills as a <color=green>Human</color>: <color=yellow>{1}</color></size>'
  scp_kill_message: '<size=27><color=#70EE9C>{0}</color> had the most kills as a <color=red>SCP</color>: <color=yellow>{1}</color></size>'
  top_damage_message: '<size=27><color=#70EE9C>{0}</color> dealt the most damage: <color=yellow>{1}</color></size>'
  first_scp_killer_message: '<size=27><color=#70EE9C>{0}</color> was the first to kill an <color=red>SCP</color></size>'
  escaper_message: '<size=27><color=#70EE9C>{0}</color> escaped first from the facility: <color=yellow>{1}:{2}</color></size>'
  # Teammates:
  enable_teammates: true
  teammate_hint_delay: 4
  teammate_hint_message: |-
    <align=left><size=28><color=#70EE9C>Your Teammates</color></size> 
    <size=25><color=yellow>{0}</color></size></align>
  teammate_message_duration: 8
  alone_hint_message: '<align=left><color=red>You are playing Solo</color></align>'
  alone_message_duration: 4
  # Last Human Broadcast:
  enable_last_human_broadcast: true
  broadcast_for_human: '<color=red>You are the last human alive!</color>'
  broadcast_for_scp: '<color=#70EE9C>{0}</color> is the last human alive, playing as {1} in <color=yellow>{2}</color>'
  ignore_tutorial_role: true
  # Map Broadcast:
  enable_map_broadcast: true
  broadcast_warning_lcz: '<color=yellow>Light Zone</color> will be decontaminated in 5 minutes!'
```
