export function SaveData(id,respondent){
    localStorage.setItem(id,JSON.stringify(respondent));
}