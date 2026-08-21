=============================
   Aegagropilon Options
=============================
* EnableDevourAttackFix
  - (Bugfix) Devour can be used once everytime he transforms into his legged form.
* ImprovedDevourTargeting (Required: EnableDevourAttackFix)
  - Relaxes the targeting requirements for Devour by removing the direction requirement and doubling the range.
* AlternateDevourEatAnimation (Required: EnableDevourAttackFix)
  - Changes the animation for eating the target from a static frame to an animation.
* BallFormAlwaysAttacks
  - Ball Form will always attack once before returning to Legged Form.
 
=============================
   Blue Dragon Options
=============================
* IncreasedAttackRange
  - Increased Blue Dragon's attack range from 64 pixels to 192.

=============================
   Brambler Options
=============================
* OutOfBoundsAndCountFix
  - (Bugfix) Bramblers no longer have a chance of spawning out side the map boundries or in other invalid positions.
  - (Bugfix) Tropicallo will now spawn exactly his limit of Bramblers (Default: 2) instead of a semi-random amount.
* WeaponFix
  - (Bugfix) Bramblers spawned by Boreal Face now equip the correct weapon.
  
=============================
   Dark Lich Options
=============================
* HatesHeavyMetalMusic
  - (Bugfix) Can attack while underground with his head sticking out.
* UndergroundHorizontalMovement
  - Can move horizontally while hiding underground.
* UseSuperMagic
  - Dark Lich gains the ability to cast super spells at a default rate of 50%.
* SuperMagicRate
  - The chance (out of 128) that Dark Lich will cast super spells.
  
=============================
   Doom's Wall Options
=============================
* CaveInNameFix
  - (Bugfix) Fixed invalid character in the name of the "Cave-In" skill.
  
=============================
   Dragon (All) Options
=============================
* SkillTargetingFix
  - (Bugfix) Dragon skills will now properly target all opponents.

=============================
   Hexas Options
=============================
* ImprovedBarrierChange
  - (Bugfix) Barrier Change can now use it's Sylphid Form.
  - (Bugfix) Barrier Change will now change Hexas's weakness and resistance elements.
  - Barrier Change will now occur after enough time has passed or after taking damage enough times.
* BarrierChangeShade (Required: ImprovedBarrierChange)
  - Hexas can change into the Shade element.
  - Can cast Evil Gate and Dark Force, or can double cast Dark Force.
* InfiniteMP
  - Sets MP to 99 and refills every movement tick.
* UseSuperMagic
  - Raises Hexas black magic level to 8 and gains the ability to cast super spells at a default rate of 25%.
* SuperMagicRate
  - The chance (out of 128) that Hexas will cast super spells.
* LunaMoogleBubbles
  - Hexas can use the skill 'Moogle Bubbles' while his barrier is set to Luna.
* SpawnElementFix
  - (Bugfix) Will now spawn with the Luna element instead of with no element.
  
=============================
   Kettle Kin Options
=============================
* RestoreDeathMachine
  - Replaces Kettle Kin with Death Machine.
  - A copy of the Seiken Densetsu 2 ROM is required.
* DrillSpinsDuringMovement (Required: RestoreDeathMachine)
  - Death Machine's drill will spin when he moves.

=============================
   Mech Rider (All) Options
=============================
* AILocksOnDamageFix
  - (Bugfix) Taking damage or dying will properly set the AI lock routine instead of attempting to freeze the current command for 13,497 ticks (67,485 frames).
* TargetAlignmentDoesntLockAI
  - Stayng north or south of Mech Rider will no longer prevent him from attacking.
* DoubleVerticalMovement
  - Mech Rider moves north and south at twice the normal rate.

=============================
   Mech Rider I Options
=============================
* IncreasedStatistics
  - Increased agility from 8 to 21.

=============================
   Mech Rider II Options
=============================
* IncreasedStatistics
  - Increased strength from 30 to 37.
  - Increased agility from 1 to 37.
  - Increased physical defense from 15 to 48.
  - Increased magic defense from 53 to 128.
  - Increased white magic power from 2 to 37.

=============================
   Mech Rider III Options
=============================
* SpellAndSkillUseFix
  - (Bugfix) Corrected spell casting logic so cannon attacks are used while Wall is active.
* DiffuserCannonTargetingFix
  - (Bugfix) Diffuser Cannon will now properly target all opponents instead of just his current target.
* IncreasedDiffuserCannonDamage
  - Increased the power of Diffuser Cannon from 69 to 109.
* IncreasedStatistics
  - Increased agility from 15 to 53.
  - Increased white magic power from 2 to 48.
  - Increased magic level from 5 to 6.

=============================
   Metal Mantis Options
=============================
* UsefulWeapons
  - Equips a properly leveled melee and projectile weapon instead of reusing Mantis Ant's.
* IncreasedFireBeamDamage
  - Increased the power of the 'Fire Beam' skill from 11 to 69.
* HasImprovedAcidBreathCommand
  - Can use a more powerful version of the 'Acid Breath' skill.
 
=============================
   Red Dragon Options
=============================
* IncreasedAttackRange
  - Increased Red Dragon's attack range from 64 pixels to 192.
* ImprovedSleepRing
  - Increased power from 0 to 96.
  - Increased status effect rate from 50% to 99%.

=============================
   Snow Dragon Options
=============================
* NewFrostWingSkill
  - Replaces Snow Dragon's 'Breath Wing' skill with a new skill 'Frost Wing'.
  - Uses the unused icicle graphics in the dragon tileset.
* IncreasedAttackRange
  - Increased Snow Dragon's attack range from 96 pixels to 192.

=============================
   Tropicallo Options
=============================
* MaxBramblerSpawns
  - Sets the total number of Brambler's that can spawn/respawn.
* InfiniteBramblerSpawns
  - No limit to the number of Brambler respawns.