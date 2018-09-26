﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zadanie
{
    class Program
    {
        //static List<int> Ai = new List<int> { 1, 4, 5, 3, 6, 2 };
        //static List<int> Bi = new List<int> { 5, 3, 2, 4, 6, 1 };
        //static List<int> Weight = new List<int> { 2400, 2000, 1200, 2400, 1600, 4000 };

        static List<int> Ai = new List<int>();//aktualne ustawienie słoni
        static List<int> Bi = new List<int>();//docelowe ustawienie słoni
        static List<int> Weight = new List<int>();//masy poszczególnych słoni

        //funkcja zwracająca index słonia z początkowego ustawieniu 
        public static int FindMe(int slon)
        {

            for (int s = 0; s < Ai.Count; s++)
            {
                if (Ai[s] == slon)
                {
                    return s;
                }
            }
            return -1;
        }

        static void Main(string[] args)
        {


            //wczytanie danych z pliku
            try
            {
                //Tutaj nazwa pliku z którego pobieramy dane
                using (StreamReader sr = new StreamReader("slo5.in"))
                {
                    string line;//wiersz z pliku
                    int line_nr = 1;//nr wiersza 
                    int liczba_sloni = 0;
                    //wczytywanie linie po liniu do końca pliku
                    while ((line = sr.ReadLine()) != null)
                    {
                        //Console.WriteLine(line);
                        string[] tab = line.Split(' '); // podzielenie wiersza wdedług spacji
                        int value;//przekonwertowana wartość stringa na int
                        for (int i = 0; i < tab.Length; i++) //wczytywanie danych do poszczególnych list
                        {
                            if (Int32.TryParse(tab[i], out value))
                            {
                                if (line_nr == 1)
                                {
                                    liczba_sloni = value; //pierwszy wiersz - liczba słoni
                                }
                                
                                if (line_nr == 2)
                                {
                                    Weight.Add(value); //drugi wiersz - wagi słoni
                                }
                                else if (line_nr == 3)
                                {
                                    Ai.Add(value); // trzeci wiersz - ustawienie początkowe słoni
                                }
                                else if (line_nr == 4)
                                {
                                    Bi.Add(value); // czwarty wiersz - ustawienie docelowe słoni
                                }
                            }
                        }
                        line_nr++;

                    }
                }
            }

            catch (Exception e)
            {
                // coś poszło nie tak
                Console.WriteLine("Nie można odczytać pliku:");
                Console.WriteLine(e.Message);
            }



            //Rozkład na cykle proste
            List<List<int>> Cykle = new List<List<int>>();//lista z Cyklami, każdy cykl to też lista
            int nr_cyklu = 0;
            do
            {
                Cykle.Add(new List<int>()); // nowy cykl 
                //Cykle[nr_cyklu].Add(Ai[j]);
                int j = 0;//numer rzędu gdzie jestem (które zostały po wykasowaniu)
                do
                {
                    Cykle[nr_cyklu].Add(Ai[j]); //dodanie do poszczególnego cyklu (podązanie po poszczególnych wierzchołkach grafu)
                    if (Ai[j] == Bi[j]) // cykl jedno-elementowy
                    {
                        Ai.RemoveAt(j); //usuwanie już użytych numerów 
                        Bi.RemoveAt(j); //usuwanie już użytych numerów 
                        break;
                    }

                    int temp = Bi[j];
                    Ai.RemoveAt(j); //usuwanie już użytych numerów 
                    Bi.RemoveAt(j); //usuwanie już użytych numerów 
                    j = FindMe(temp); // znajdź jindeks gdzie iść 

                } while (j!=-1); //aż wrócimy do tego samego wierzchołka - koniec cyklu
                nr_cyklu++;
            } while (Ai.Count != 0); // wykorzystanie wszystkich danych

            // Liczenie parametrów kryteriów
            int min = Weight.Min();
            int E1, E2, E;
            E1 = 0;E2 = 0;E = 0;
            foreach (List<int> Cykl in Cykle)
            {
                int suma = 0;
                int minC = Weight[Cykl[0]-1];//na początku najlżejszy 1 słoń cyklu
                int C = Cykl.Count;
                for (int nr = 0; nr < C; nr++)
                {
                    suma = suma + Weight[Cykl[nr]-1];
                    if (Weight[Cykl[nr]-1] < minC)
                    {
                        minC = Weight[Cykl[nr]-1];
                    }
                }
                E1 = suma + (C - 2) * minC;
                E2 = suma + minC + (C + 1) * min;
                int Emin = E1;
                if(E2<Emin)
                {
                    Emin = E2;
                }
                
                E = E + Emin; // Energia potrzebna do przestawienia słoni
            }
            Console.WriteLine(E);
            Console.ReadKey();
        }
    }
}

