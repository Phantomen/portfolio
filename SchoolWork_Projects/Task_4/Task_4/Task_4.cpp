#include "pch.h"
#include <iostream>
#include <math.h>
#include <vector>
#include <chrono>

using namespace std;

int n = 100;


//int T1(int n)
//{
//	if (n == 1)
//		return 1;
//
//	return (T1(n - 1) + T1((int)ceil(n / 2)) + n);
//}
//
//void Main_T1()
//{
//	cout << "Trivial: " << T1(n) << endl;
//}


vector<int> v;

int T2(int n)
{
	if (v[n - 1] == NULL)
	{
		v[n - 1] = T2(n - 1) + T2((int)ceil(n / 2)) + n;
	}

	return v[n - 1];
}

void Main_T2()
{
	v = vector<int>(n);

	v[0] = 1;

	cout << "Dynamic: " << T2(n) << endl;

	//system("pause");
}



int main()
{
	//Main_T1();
	//Main_T2();

	/*DEBUG*/int runTest = 50;/*DEBUG*/
	/*DEBUG*/double medelTid = 0;/*DEBUG*/


	//vector<int> v;
	for (int times = 0; times < runTest; times++)
	{
		/*DEBUG*/chrono::high_resolution_clock::time_point start = chrono::high_resolution_clock::now();/*DEBUG*/

		//Main_T1();

		Main_T2();


		/*DEBUG*/chrono::high_resolution_clock::time_point end = chrono::high_resolution_clock::now();/*DEBUG*/
		/*DEBUG*/chrono::duration<double> elipse = chrono::duration_cast<chrono::duration<double>>(end - start);/*DEBUG*/
		/*DEBUG*/medelTid = elipse.count() + medelTid;/*DEBUG*/
		/*DEBUG*/cout << times << "/ " << runTest << " " << "completion Time " << elipse.count() << " seconds (" << n << ")" << endl;/*DEBUG*/
		
		v.clear();
	}

	cout << endl;
	/*DEBUG*/cout << "medeltid för " << n << " är " << (medelTid / runTest) << " seconds " << endl;/*DEBUG*/

	char wait;

	cin >> wait;

	n += 100;
	main();

	return 0;
}