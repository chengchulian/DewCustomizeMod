using System.Collections.Generic;
using DewCustomizeMod.config;
using DewCustomizeMod.i18n;
using UnityEngine;

namespace DewCustomizeMod.ui
{
    public static class ConfigEditorUI
    {
        private static readonly GUIStyle FoldoutStyle = new()
        {
            fontStyle = FontStyle.Bold,
            fontSize = 14,
            normal = { textColor = Color.red },
            onNormal = { textColor = Color.black }
        };

        private static bool _showGeneralSettings = true;
        private static bool _showEnemySettings = true;
        private static bool _showBossSettings = true;
        private static bool _showSkillSettings = true;
        private static bool _showGameplayToggles = true;
        private static bool _showSkillAndGemInGameSettings = true;
        
        

        private static AttrCustomizeConfig _config;

        public static void DrawConfigFields()
        {
            _config = AttrCustomizeResources.Config;

            GUILayout.Space(10);

            // 通用设置
            _showGeneralSettings = GUILayout.Toggle(_showGeneralSettings, LocalizationConfig.Get("section_general"), FoldoutStyle, GUILayout.Height(25));
            if (_showGeneralSettings)
            {
                
                ConfigFieldDrawer.DrawIntField(LocalizationConfig.Get("label_max_players"), ref _config.maxPlayer);
                ConfigFieldDrawer.DrawFloatField(LocalizationConfig.Get("label_population_multiplier"), ref _config.maxAndSpawnedPopulationMultiplier);
                ConfigFieldDrawer.DrawIntField(LocalizationConfig.Get("label_shop_added_items"), ref _config.shopAddedItems);
                ConfigFieldDrawer.DrawIntField(LocalizationConfig.Get("label_shop_refreshes"), ref _config.shopRefreshes);
                ConfigFieldDrawer.DrawIntField(LocalizationConfig.Get("label_first_visit_gold"), ref _config.firstVisitDropGoldCount);
                ConfigFieldDrawer.DrawIntField(LocalizationConfig.Get("label_first_visit_gold_loop"), ref _config.firstVisitDropGoldCountAddByLoop);
                ConfigFieldDrawer.DrawIntField(LocalizationConfig.Get("label_first_visit_gold_zone"), ref _config.firstVisitDropGoldCountAddByZone);
                ConfigFieldDrawer.DrawFloatField(LocalizationConfig.Get("label_beneficial_node_multiplier"), ref _config.beneficialNodeMultiplier);
                ConfigFieldDrawer.DrawPercentageField(LocalizationConfig.Get("label_heal_raw_multiplier"), ref _config.healRawMultiplier);
                ConfigFieldDrawer.DrawIntField(LocalizationConfig.Get("label_num_of_nodes"), ref _config.numOfNodes);
                ConfigFieldDrawer.DrawIntField(LocalizationConfig.Get("label_num_of_merchants"), ref _config.numOfMerchants);

            }

            GUILayout.Space(10);

            // 敌人设置
            _showEnemySettings = GUILayout.Toggle(_showEnemySettings, LocalizationConfig.Get("section_enemy"), FoldoutStyle, GUILayout.Height(25));
            if (_showEnemySettings)
            {
                ConfigFieldDrawer.DrawPercentageField(LocalizationConfig.Get("label_enemy_movement_speed"), ref _config.enemyMovementSpeedPercentage);
                ConfigFieldDrawer.DrawPercentageField(LocalizationConfig.Get("label_enemy_attack_speed"), ref _config.enemyAttackSpeedPercentage);
                ConfigFieldDrawer.DrawPercentageField(LocalizationConfig.Get("label_enemy_ability_haste"), ref _config.enemyAbilityHasteFlat);
                ConfigFieldDrawer.DrawPercentageField(LocalizationConfig.Get("label_little_monster_health"), ref _config.littleMonsterHealthMultiplier);
                ConfigFieldDrawer.DrawPercentageField(LocalizationConfig.Get("label_little_monster_damage"), ref _config.littleMonsterDamageMultiplier);
                ConfigFieldDrawer.DrawPercentageField(LocalizationConfig.Get("label_miniboss_health"), ref _config.miniBossHealthMultiplier);
                ConfigFieldDrawer.DrawPercentageField(LocalizationConfig.Get("label_miniboss_damage"), ref _config.miniBossDamageMultiplier);
                ConfigFieldDrawer.DrawPercentageField(LocalizationConfig.Get("label_enemy_health_growth"), ref _config.extraHealthGrowthMultiplier);
                GUILayout.Label(LocalizationConfig.Get("label_enemy_health_growth_example"));
                ConfigFieldDrawer.DrawPercentageField(LocalizationConfig.Get("label_enemy_damage_growth"), ref _config.extraDamageGrowthMultiplier);
                ConfigFieldDrawer.DrawFloatField(LocalizationConfig.Get("label_monster_mirage_chance"), ref _config.monsterMirageChanceMultiple);
            }

            GUILayout.Space(10);

            // Boss 设置
            _showBossSettings = GUILayout.Toggle(_showBossSettings, LocalizationConfig.Get("section_boss"), FoldoutStyle, GUILayout.Height(25));
            if (_showBossSettings)
            {
                ConfigFieldDrawer.DrawIntField(LocalizationConfig.Get("label_boss_count"), ref _config.bossCount);
                ConfigFieldDrawer.DrawIntField(LocalizationConfig.Get("label_boss_count_loop"), ref _config.bossCountAddByLoop);
                ConfigFieldDrawer.DrawIntField(LocalizationConfig.Get("label_boss_count_zone"), ref _config.bossCountAddByZone);
                ConfigFieldDrawer.DrawPercentageField(LocalizationConfig.Get("label_boss_health"), ref _config.bossHealthMultiplier);
                ConfigFieldDrawer.DrawPercentageField(LocalizationConfig.Get("label_boss_damage"), ref _config.bossDamageMultiplier);
                ConfigFieldDrawer.DrawPercentageField(LocalizationConfig.Get("label_boss_injury_limit"), ref _config.bossSingleInjuryHealthMultiplier);
                ConfigFieldDrawer.DrawPercentageField(LocalizationConfig.Get("label_boss_mirage_chance"), ref _config.bossMirageChance);
                ConfigFieldDrawer.DrawPercentageField(LocalizationConfig.Get("label_boss_hunter_chance"), ref _config.bossHunterChance);
            }

            GUILayout.Space(10);

            // 技能与精华槽设置
            _showSkillSettings = GUILayout.Toggle(_showSkillSettings, LocalizationConfig.Get("section_skillslot"), FoldoutStyle, GUILayout.Height(25));
            if (_showSkillSettings)
            {
                ConfigFieldDrawer.DrawIntField(LocalizationConfig.Get("label_skill_q_gem_count"), ref _config.skillQGemCount);
                ConfigFieldDrawer.DrawIntField(LocalizationConfig.Get("label_skill_w_gem_count"), ref _config.skillWGemCount);
                ConfigFieldDrawer.DrawIntField(LocalizationConfig.Get("label_skill_e_gem_count"), ref _config.skillEGemCount);
                ConfigFieldDrawer.DrawIntField(LocalizationConfig.Get("label_skill_r_gem_count"), ref _config.skillRGemCount);
                ConfigFieldDrawer.DrawIntField(LocalizationConfig.Get("label_skill_identity_gem_count"), ref _config.skillIdentityGemCount);
                ConfigFieldDrawer.DrawIntField(LocalizationConfig.Get("label_skill_movement_gem_count"), ref _config.skillMovementGemCount);
                _config.enableGemMerge = GUILayout.Toggle(_config.enableGemMerge, LocalizationConfig.Get("label_enable_gem_merge"));
            }

            GUILayout.Space(10);

            // 游戏机制开关
            _showGameplayToggles = GUILayout.Toggle(_showGameplayToggles, LocalizationConfig.Get("section_toggles"), FoldoutStyle, GUILayout.Height(25));
            if (_showGameplayToggles)
            {
                _config.enableHeroSkillAddShop = GUILayout.Toggle(_config.enableHeroSkillAddShop, LocalizationConfig.Get("label_enable_hero_skill_add_shop"));
                _config.enableMistAllowAnyDirection = GUILayout.Toggle(_config.enableMistAllowAnyDirection, LocalizationConfig.Get("label_enable_mist_allow_any_direction"));
                _config.enableWorldReveal = GUILayout.Toggle(_config.enableWorldReveal, LocalizationConfig.Get("label_enable_world_reveal"));
                _config.enableBossSpawnAllOnce = GUILayout.Toggle(_config.enableBossSpawnAllOnce, LocalizationConfig.Get("label_enable_boss_spawn_all_once"));
                _config.enableArtifactQuest = GUILayout.Toggle(_config.enableArtifactQuest, LocalizationConfig.Get("label_enable_artifact_quest"));
                _config.enableFragmentOfRadianceBossQuest = GUILayout.Toggle(_config.enableFragmentOfRadianceBossQuest, LocalizationConfig.Get("label_enable_fragment_of_radiance_boss_quest"));
                _config.enableHealthReduceMultiplierAddByZone = GUILayout.Toggle(_config.enableHealthReduceMultiplierAddByZone, LocalizationConfig.Get("label_enable_health_reduce_multiplier_add_by_zone"));
                _config.enableCurrentNodeGenerateLostSoul = GUILayout.Toggle(_config.enableCurrentNodeGenerateLostSoul, LocalizationConfig.Get("label_enable_current_node_generate_lost_soul"));
                _config.enableBossRoomGenerateLostSoul = GUILayout.Toggle(_config.enableBossRoomGenerateLostSoul, LocalizationConfig.Get("label_enable_boss_room_generate_lost_soul"));
                _config.enableQuestHuntedByObliviaxRepeatable = GUILayout.Toggle(_config.enableQuestHuntedByObliviaxRepeatable, LocalizationConfig.Get("label_enable_quest_hunted_by_obliviax_repeatable"));
                _config.enableDamageRanking = GUILayout.Toggle(_config.enableDamageRanking, LocalizationConfig.Get("label_enable_damage_ranking"));
                _config.enableLucidDreamEmbraceMortality = GUILayout.Toggle(_config.enableLucidDreamEmbraceMortality, LocalizationConfig.Get("label_enable_lucid_dream_embrace_mortality"));
                _config.enableLucidDreamBonVoyage = GUILayout.Toggle(_config.enableLucidDreamBonVoyage, LocalizationConfig.Get("label_enable_lucid_dream_bon_voyage"));
                _config.enableLucidDreamGrievousWounds = GUILayout.Toggle(_config.enableLucidDreamGrievousWounds, LocalizationConfig.Get("label_enable_lucid_dream_grievous_wounds"));
                _config.enableLucidDreamTheDarkestUrge = GUILayout.Toggle(_config.enableLucidDreamTheDarkestUrge, LocalizationConfig.Get("label_enable_lucid_dream_the_darkest_urge"));
                _config.enableLucidDreamWild = GUILayout.Toggle(_config.enableLucidDreamWild, LocalizationConfig.Get("label_enable_lucid_dream_wild"));
                _config.enableLucidDreamMadLife = GUILayout.Toggle(_config.enableLucidDreamMadLife, LocalizationConfig.Get("label_enable_lucid_dream_mad_life"));
                _config.enableLucidDreamSparklingDreamFlask = GUILayout.Toggle(_config.enableLucidDreamSparklingDreamFlask, LocalizationConfig.Get("label_enable_lucid_dream_sparkling_dream_flask"));
            }

            GUILayout.Space(10);

            // 游戏内资源相关
            _showSkillAndGemInGameSettings = GUILayout.Toggle(_showSkillAndGemInGameSettings, LocalizationConfig.Get("section_resources"), FoldoutStyle, GUILayout.Height(25));
            if (_showSkillAndGemInGameSettings)
            {
                ConfigArrayDrawer.DrawStartSkillAndLevelArray(ref _config.startSkills, ref _config.startSkillsLevel);
                ConfigArrayDrawer.DrawStartGemAndQualityArray(ref _config.startGems, ref _config.startGemsQuality);
                ConfigArrayDrawer.DrawRemoveSkillArray(ref _config.removeSkills);
                ConfigArrayDrawer.DrawRemoveGemArray(ref _config.removeGems);
            }
            

        }
    }
}
