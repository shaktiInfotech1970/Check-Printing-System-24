#include<bits/stdc++.h>
using namespace std;
int main(){
	int n;
	cin>>n;
	vector<int> arr(n);
	for(int i=0;i<n;i++){
		cin>>arr[i];
	}
    cout<<"Arya Shah"<<endl;
    cout<<"Enrollment No : 220410107103"<<endl;

	cout<<"Before Swapping : "<<endl;
	for(auto it:arr){
		cout<<it<<" ";
	}
	cout<<endl;
	for(int i=n-1;i>=0;i--){
		for(int j=0;j<=i-1;j++){
			if(arr[j]>arr[j+1]){
				swap(arr[j],arr[j+1]);
			}
		}
	}
	cout<<"After Swapping using Bubble Sort : "<<endl;
	for(auto it:arr){
		cout<<it<<" ";
	}
	cout<<endl;
}
