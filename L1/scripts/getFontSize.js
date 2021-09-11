export function GetFontSize(a,b,isRandom){
    return isRandom ? Math.floor(Math.random() * (b-a))+a : 14; 
}