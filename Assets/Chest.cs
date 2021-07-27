using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chest : MonoBehaviour
{

    System.Random rand = new System.Random();
    void Start()
    {
        List<Item> itemList = InitializeItemList();
        OpenMultipleChests(10000, itemList);
        print(TestA(itemList));
        
        print(TestC(itemList));
        print(TestD(itemList));
        print(TestE(itemList));
    }


    //Если монета, руда и аптечка могут выпасть только один раз, то сколько нужно открыть ящиков, чтобы получить пистолет?
    public float TestD(List<Item> itemList)
    {
        print("test d:");
        int chestsOpened = 0;
        int testCount = 1000000;

        for (int i = 0; i < testCount; i++)
        {
            List<Item> currentItemList = new List<Item>(itemList);
            while (true)
            {
                Item droppedItem = OpenChest(currentItemList);
                chestsOpened++;
                if (droppedItem.itemType == ItemType.Pistol)
                    break;
                if (droppedItem.itemType == ItemType.Coin || droppedItem.itemType == ItemType.Ore || droppedItem.itemType == ItemType.Medkit)
                    currentItemList.Remove(droppedItem);
            }
        }
        
        return chestsOpened / (float)testCount;
    }


    //Сколько человек из тысячи получит три монеты, открыв первые пять ящиков?
    public float TestE(List<Item> itemList)
    {
        print("test e:");
        int peopleCount = 1000000;
        int chestCount = 5;
        int threeCoinPeopleCount = 0;

        for (int p = 0; p < peopleCount; p++)
        {
            int coinCount = 0;
            for (int c = 0; c < chestCount; c++)
            {
                Item droppedItem = OpenChest(itemList);
                if (droppedItem.itemType == ItemType.Coin)
                    coinCount++;
            }
            if (coinCount == 3)
                threeCoinPeopleCount++;
        }
        print("threeCoinPeopleCount fraction = " + threeCoinPeopleCount / (float)peopleCount);
        return threeCoinPeopleCount / (float)peopleCount * 1000;
    }

    //Какова вероятность, что из первых ста открытых ящиков не выпадет ни одного пистолета?
    public float TestC(List<Item> itemList)
    {
        print("test c:");
        int testCount = 100000;
        int chestCount = 100;
        int pistolDropCount = 0;

        for (int t = 0; t < testCount; t++)
        {
            for (int c = 0; c < chestCount; c++)
            {
                Item droppedItem = OpenChest(itemList);
                if (droppedItem.itemType == ItemType.Pistol)
                {
                    pistolDropCount++;
                    break;
                }
            }
        }
        return (testCount - pistolDropCount) / (float)testCount;
    }

    

    private List<Item> InitializeItemList()
    {
        List<Item> itemList = new List<Item>();
        itemList.Add(new Item(ItemType.Pistol, 1));
        itemList.Add(new Item(ItemType.Helmet, 5));
        itemList.Add(new Item(ItemType.Ore, 15));
        itemList.Add(new Item(ItemType.Medkit, 25));
        itemList.Add(new Item(ItemType.Coin, 100));
        return itemList;
    }

    void OpenMultipleChests(int number, List<Item> itemList)
    {
        print("initial probability destibution test:");
        Dictionary<Item, int> itemCounter = new Dictionary<Item, int>();
        for (int i = 0; i < number; i++)
        {
            Item rolledItem = OpenChest(itemList);
            //print(rolledItem.itemType);
            if (itemCounter.ContainsKey(rolledItem))
                itemCounter[rolledItem] += 1;
            else
                itemCounter.Add(rolledItem, 1);
        }

        foreach (KeyValuePair<Item, int> itemCount in itemCounter)
        {
            print(itemCount.Key.itemType + " : " + itemCount.Value);
        }
    }

    Item OpenChest(List<Item> itemList)
    {
        int chanceMultSum = 0;
        int minLimit = 0;
        int maxLimit = 0;

        foreach (Item item in itemList)
        {
            chanceMultSum += item.chanceMultiplier;
        }

        //print("sum of multipliers: " + chanceMultSum);

        //random number from 0 to chanceMultSum
        int rolledInt = rand.Next(0, chanceMultSum);
        //print(rolledInt);

        foreach (Item item in itemList)
        {
            maxLimit += item.chanceMultiplier;
            if (rolledInt >= minLimit && rolledInt < maxLimit)
            {
                return item;
            }
            minLimit += item.chanceMultiplier;
        }
        throw new UnityException("WTF, no item dropped");
    }


    // Если ящик открывает один и тот же человек, сколько руды будет у него, когда выпадет шлем?
    public float TestA(List<Item> items)
    {
        print("test a:");
        int oreCount = 0;
        int testCount = 100000;
        
        for (int i = 0; i < testCount; i++)
        {
            bool isHelmetDroped = false;
            while (isHelmetDroped == false)
            {
                Item droppedItem = OpenChest(items);
                if (droppedItem.itemType == ItemType.Ore)
                    oreCount++;
                else if (droppedItem.itemType == ItemType.Helmet)
                    isHelmetDroped = true;
            }

        }
        
        return oreCount / (float)testCount;
    }
}
