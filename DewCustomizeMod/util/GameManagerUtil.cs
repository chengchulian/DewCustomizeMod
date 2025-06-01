using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using DewCustomizeMod.config;
using HarmonyLib;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DewCustomizeMod.util;

public class GameManagerUtil
{
    public static void ExecGameStartLoadBefore(GameManager gameManager)
    {
        ModifyZoneData();
    }

    public static void ExecGameStartLoadAfter(GameManager gameManager)
    {
        
        //挂载发钱
        NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnRoomLoaded += FirstVisitDropGold;
        
        //挂载遗忘猎手任务重复生成
        NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnRoomLoaded += QuestHuntedByObliviaxRepeatable;

        //挂载移除RoomMod事件
        NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnZoneLoaded += RemoveRoomMod;
        
        //挂载透视
        NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnZoneLoaded += WorldReveal;    
        
        //挂载伤害排行榜
        NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnZoneLoaded += DamageRanking;


        LucidDreamEmbraceMortality();
        LucidDreamBonVoyage();
        LucidDreamGrievousWounds();
        LucidDreamTheDarkestUrge();
        LucidDreamWild();
        LucidDreamMadLife();
        LucidDreamSparklingDreamFlask();

    }
    
    
    private static void LucidDreamSparklingDreamFlask()
    {
        if (AttrCustomizeResources.Config.enableLucidDreamSparklingDreamFlask)
        {
            NetworkedManagerBase<ActorManager>.instance.onActorAdd +=
                new Action<Actor>(LucidDreamSparklingDreamFlaskOnActorAdd);
            foreach (Actor a in NetworkedManagerBase<ActorManager>.instance.allActors)
            {
                LucidDreamSparklingDreamFlaskOnActorAdd(a);
            }
        }
    }

    
    private static void LucidDreamSparklingDreamFlaskOnActorAdd(Actor obj)
    {
        Ai_RegenOrb_Projectile ai_RegenOrb_Projectile = obj as Ai_RegenOrb_Projectile;
        if (ai_RegenOrb_Projectile != null)
        {
            ai_RegenOrb_Projectile.actionOverride = delegate(Entity target)
            {
                float num = Shrine_Hatred.GetBaseRewardAmount_DreamDust() * Random.Range(0.9f, 1.1f) * 0.4f;
                NetworkedManagerBase<PickupManager>.instance.DropDreamDust(false, DewMath.RandomRoundToInt(num), target.agentPosition, (Hero)target);
            };
        }
        Shrine_Guidance shrine = obj as Shrine_Guidance;
        if (shrine != null)
        {
            shrine.actionOverride = delegate(Entity target)
            {
                float num2 = Shrine_Hatred.GetBaseRewardAmount_DreamDust() * Random.Range(0.9f, 1.1f) * 1.2f;
                NetworkedManagerBase<PickupManager>.instance.DropDreamDust(false, DewMath.RandomRoundToInt(num2), shrine.position, (Hero)target);
            };
        }
    }

    private static void LucidDreamMadLife()
    {
        if (AttrCustomizeResources.Config.enableLucidDreamMadLife)
        {
            NetworkedManagerBase<GameManager>.instance.predictionStrengthOverride =
                () => 0.8f + Random.value * 0.2f;
        }
    }

    
    
    private static void LucidDreamWild()
    {
        if (AttrCustomizeResources.Config.enableLucidDreamWild)
        {
            NetworkedManagerBase<ActorManager>.instance.onEntityAdd += new Action<Entity>(LucidDreamWildOnEntityAdd);
        }
    }

    private static void LucidDreamWildOnEntityAdd(Entity obj)
    {
        if (obj is Monster && !(obj.owner != DewPlayer.creep) && !obj.IsAnyBoss() &&
            !obj.Status.HasStatusEffect<Se_HunterBuff>())
        {
            NetworkedManagerBase<ActorManager>.instance.serverActor
                .CreateStatusEffect<Se_HunterBuff>(obj, new CastInfo(obj, obj)).enableGoldAndExpDrops = true;
            DewResources.GetByType<RoomMod_Hunted>()
                .ApplyHunterStatBonusAndAIPrediction(obj, NetworkedManagerBase<ZoneManager>.instance.currentHuntLevel);
        }
    }

    
    private static void LucidDreamTheDarkestUrge()
    {
        if (AttrCustomizeResources.Config.enableLucidDreamTheDarkestUrge)
        {
            DewPlayer.creep.enemies.Add(DewPlayer.creep);
            foreach (DewPlayer x in DewPlayer.humanPlayers)
            {
                foreach (DewPlayer y in DewPlayer.humanPlayers)
                {
                    if (!(x == y))
                    {
                        x.neutrals.Add(y);
                    }
                }
            }

            NetworkedManagerBase<ActorManager>.instance.onEntityAdd +=
                new Action<Entity>(LucidDreamTheDarkestUrgeOnEntityAdd);
            foreach (Entity e in NetworkedManagerBase<ActorManager>.instance.allEntities)
            {
                LucidDreamTheDarkestUrgeOnEntityAdd(e);
            }
        }
    }
    private static void LucidDreamTheDarkestUrgeOnEntityAdd(Entity entity)
    {
        if (entity is Monster m)
        {
            m.ActorEvent_OnKill += new Action<EventInfoKill>(LucidDreamTheDarkestUrgeActorEventOnKill);
        }
    }
    private class Ad_MonsterLevelUp
    {
        public StatBonus bonus;
    }

    private static void LucidDreamTheDarkestUrgeActorEventOnKill(EventInfoKill obj)
    {
        Entity killer = obj.actor.firstEntity;
        if (!(killer == null) && !(obj.victim == null) && obj.victim is Monster)
        {
            if (!killer.TryGetData<Ad_MonsterLevelUp>(out var data))
            {
                data = new Ad_MonsterLevelUp
                {
                    bonus = killer.Status.AddStatBonus(new StatBonus())
                };
                killer.AddData(data);
            }

            if (obj.victim.TryGetData<Ad_MonsterLevelUp>(out var victimData))
            {
                data.bonus.attackDamagePercentage += victimData.bonus.attackDamagePercentage;
                data.bonus.abilityPowerPercentage += victimData.bonus.abilityPowerPercentage;
                data.bonus.attackSpeedPercentage += victimData.bonus.attackSpeedPercentage;
                data.bonus.movementSpeedPercentage += victimData.bonus.movementSpeedPercentage;
                data.bonus.abilityHasteFlat += victimData.bonus.abilityHasteFlat;
            }

            data.bonus.maxHealthFlat += obj.victim.maxHealth * 0.8f;
            data.bonus.attackDamagePercentage += 25f;
            data.bonus.abilityPowerPercentage += 25f;
            data.bonus.attackSpeedPercentage += 10f;
            data.bonus.movementSpeedPercentage += 10f;
            data.bonus.abilityHasteFlat += 15f;
            killer.Heal(obj.victim.maxHealth * 0.8f).Dispatch(killer);
        }
    }
    private static void LucidDreamGrievousWounds()
    {
        if (AttrCustomizeResources.Config.enableLucidDreamGrievousWounds)
        {
            NetworkedManagerBase<ActorManager>.instance.onEntityAdd +=
                new Action<Entity>(LucidDreamGrievousWoundsOnEntityAdd);
            foreach (Entity e in NetworkedManagerBase<ActorManager>.instance.allEntities)
            {
                LucidDreamGrievousWoundsOnEntityAdd(e);
            }
        }
    }
    private static void LucidDreamGrievousWoundsOnEntityAdd(Entity entity)
    {
        if (entity is Hero)
        {
            entity.takenHealProcessor.Add(LucidDreamGrievousWoundsHealProcessor, 200);
            entity.takenShieldProcessor.Add(LucidDreamGrievousWoundsShieldProcessor);
        }
    }
    private static void LucidDreamGrievousWoundsHealProcessor(ref HealData data, Actor actor, Entity target)
    {
        data.ApplyRawMultiplier(0.5f);
    }

    private static void LucidDreamGrievousWoundsShieldProcessor(ref HealData data, Actor actor, Entity target)
    {
        data.ApplyRawMultiplier(0.5f);
    }

    
    
    private static void LucidDreamBonVoyage()
    {
        if (AttrCustomizeResources.Config.enableLucidDreamBonVoyage)
        {
            NetworkedManagerBase<ZoneManager>.instance.isHuntAdvanceDisabled = true;
            NetworkedManagerBase<ZoneManager>.instance.hunterStartNodeIndex = -1;
            NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnRoomLoaded +=
                new Action<EventInfoLoadRoom>(LucidDreamBonVoyageClientEventOnRoomLoaded);
        }
    }

    
    private static void LucidDreamBonVoyageClientEventOnRoomLoaded(EventInfoLoadRoom obj)
    {
        NetworkedManagerBase<ZoneManager>.instance.isHuntAdvanceDisabled = true;
        NetworkedManagerBase<ZoneManager>.instance.hunterStartNodeIndex = -1;
    }

    
    private static void LucidDreamEmbraceMortality()
    {
        if (AttrCustomizeResources.Config.enableLucidDreamEmbraceMortality)
        {
            NetworkedManagerBase<ActorManager>.instance.onEntityAdd +=
                new Action<Entity>(LucidDreamEmbraceMortalityOnEntityAdd);
            foreach (Entity e in NetworkedManagerBase<ActorManager>.instance.allEntities)
            {
                LucidDreamEmbraceMortalityOnEntityAdd(e);
            }
        }
    }
    private static void LucidDreamEmbraceMortalityOnEntityAdd(Entity entity)
    {
        entity.dealtDamageProcessor.Add(LucidDreamEmbraceMortalityProcessor);
    }
    private static void LucidDreamEmbraceMortalityProcessor(ref DamageData data, Actor actor, Entity target)
    {
        Entity attacker = actor.firstEntity;
        if (!(attacker == null) && !(target == null) && !(attacker == target))
        {
            data.ApplyAmplification(1f);
        }
    }


    private static void ModifyZoneData()
    {
        if (AttrCustomizeResources.Config.numOfNodes < 0 && AttrCustomizeResources.Config.numOfMerchants < 0) return;
        
        
        IEnumerable<Zone> findAllByType = DewResources.FindAllByNameSubstring<Zone>("Zone_");

        foreach (var zone in findAllByType)
        {
            if (AttrCustomizeResources.Config.numOfNodes>=0)
            {
                zone.numOfNodes = new Vector2Int(Math.Max(AttrCustomizeResources.Config.numOfNodes, 2), Math.Max(AttrCustomizeResources.Config.numOfNodes, 2));
            }

            if (AttrCustomizeResources.Config.numOfMerchants>=0)
            {
                zone.numOfMerchants = new Vector2Int(Math.Max(AttrCustomizeResources.Config.numOfMerchants, 0), Math.Max(AttrCustomizeResources.Config.numOfMerchants, 0));
            }
            
        }
        if (AttrCustomizeResources.Config.numOfNodes>=0)
        {
            DewBuildProfile.current.worldNodeCountOffset = 0;        
        }
    }

    private static void WorldReveal()
    {
        if (AttrCustomizeResources.Config.enableWorldReveal)
        {
            ConsoleCommands.WorldRevealFull();
        }
    }

    
    private static void DamageRanking()
    {
        if (!AttrCustomizeResources.Config.enableDamageRanking) return;

        if (NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex == 0)
        {
            return;
        }
        
        DewGameResult tracked = (DewGameResult)typeof(GameResultManager)
            .GetField("_tracked", BindingFlags.Instance | BindingFlags.NonPublic)
            ?.GetValue(NetworkedManagerBase<GameResultManager>.instance);

        if (tracked != null)
        {
            List<ValueTuple<string, float, float>> dmgList = new List<ValueTuple<string, float, float>>();

            // 使用 for 循环遍历玩家数据
            for (int i = 0; i < tracked.players.Count; i++)
            {
                DewGameResult.PlayerData playerData = tracked.players[i];
                string playerProfileName = playerData.playerProfileName;
                float totalDmg = playerData.dealtDamageToEnemies;
                float maxDmg = playerData.maxDealtSingleDamageToEnemy;
                dmgList.Add(ValueTuple.Create(playerProfileName, totalDmg, maxDmg));
            }

            // 按总伤害降序排序（显式委托）
            dmgList.Sort(delegate(ValueTuple<string, float, float> a, ValueTuple<string, float, float> b)
            {
                return b.Item2.CompareTo(a.Item2); // 降序比较
            });

            StringBuilder sb = new StringBuilder();
            sb.Append("伤害排行\n");

            // 使用 for 循环遍历伤害列表
            for (int j = 0; j < dmgList.Count; j++)
            {
                ValueTuple<string, float, float> valueTuple = dmgList[j];
                string playerProfileName2 = valueTuple.Item1;
                float totalDmg2 = valueTuple.Item2;
                float maxDmg2 = valueTuple.Item3;
                string totalDmgFormatted = totalDmg2.ToString("#,0", CultureInfo.InvariantCulture);
                string maxDmgFormatted = maxDmg2.ToString("#,0", CultureInfo.InvariantCulture);
                sb.Append(playerProfileName2 + ": 总伤害 " + totalDmgFormatted + " | 最强一击 " + maxDmgFormatted);
                sb.Append('\n');
            }

            // 延迟发送消息（使用显式委托）
            Dew.CallDelayed(delegate
            {
                ChatManager.Message message = new ChatManager.Message();
                message.type = ChatManager.MessageType.Raw;
                message.content = sb.ToString();
                NetworkedManagerBase<ChatManager>.instance.BroadcastChatMessage(message);
            }, 100);
        }
    }

    
    private static void QuestHuntedByObliviaxRepeatable(EventInfoLoadRoom obj)
    {
        if (!AttrCustomizeResources.Config.enableQuestHuntedByObliviaxRepeatable)
        {
            return;
        }

        GameMod_Obliviax gameModObliviax = Dew.FindActorOfType<GameMod_Obliviax>();

        // 创建访问 _lastObliviaxQuestZoneIndexRef 的字段引用
        AccessTools.FieldRef<GameMod_Obliviax, int> _lastObliviaxQuestZoneIndexRef =
            AccessTools.FieldRefAccess<GameMod_Obliviax, int>("_lastObliviaxQuestZoneIndex");

        int lastObliviaxQuestZoneIndex = _lastObliviaxQuestZoneIndexRef(gameModObliviax);


        if (NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex == lastObliviaxQuestZoneIndex
            || !NetworkedManagerBase<ZoneManager>.instance.isCurrentNodeHunted
            || lastObliviaxQuestZoneIndex < 0)
        {
            return;
        }

        NetworkedManagerBase<QuestManager>.instance.StartQuest<Quest_HuntedByObliviax>();
        _lastObliviaxQuestZoneIndexRef(gameModObliviax) =
            NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex;
    }

    private static void RemoveRoomMod()
    {
        List<WorldNodeData> worldNodeDatas = NetworkedManagerBase<ZoneManager>.instance.nodes.ToList();

        if (!AttrCustomizeResources.Config.enableArtifactQuest)
        {
            for (int i = 0; i < worldNodeDatas.Count; i++)
            {
                NetworkedManagerBase<ZoneManager>.instance.RemoveModifier<RoomMod_Artifact>(i);
            }
        }

        if (!AttrCustomizeResources.Config.enableFragmentOfRadianceBossQuest)
        {
            for (int i = 0; i < worldNodeDatas.Count; i++)
            {
                NetworkedManagerBase<ZoneManager>.instance.RemoveModifier<RoomMod_FragmentOfRadiance_StartProp>(i);
            }
        }
    }


    private static void FirstVisitDropGold(EventInfoLoadRoom obj)
    {
        if (SingletonDewNetworkBehaviour<Room>.instance.isRevisit)
        {
            return;
        }

        int zoneAddCount = (NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex) *
                           AttrCustomizeResources.Config.firstVisitDropGoldCountAddByZone;
        int loopAddCount = (NetworkedManagerBase<ZoneManager>.instance.loopIndex) *
                           AttrCustomizeResources.Config.firstVisitDropGoldCountAddByLoop;
        int count = AttrCustomizeResources.Config.firstVisitDropGoldCount;
        int value = count + loopAddCount + zoneAddCount;
        foreach (var humanPlayer in DewPlayer.humanPlayers)
        {
            NetworkedManagerBase<PickupManager>.instance.DropGold(false, false, Mathf.RoundToInt(value),
                humanPlayer.hero.position, humanPlayer.hero);
        }
    }
}