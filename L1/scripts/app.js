import {fruits} from './fruits.js';
import {romes} from './romeNums.js';
import {GetFontSize} from './getFontSize.js';
import {SaveData} from './SaveData.js';

let rightAnswerArr = [];
let respondentAnswer = [];
let symbolsCount = 6;

let $select = $('.form-select').on('click',()=>{
    $('.alert').remove()
    if($select.val()==='1'){
        $('.input-form').append(`<div class="alert alert-primary" role="alert">
             У вас будет 1.5 секунды, чтобы запомнить ${symbolsCount} римских цифр, после чего необходимо отметить запомненные вами цифры.
        </div>`);
    }else if($select.val()==='2'){
        $('.input-form').append(`<div class="alert alert-primary" role="alert">
             У вас будет 1.5 секунды, чтобы запомнить ${symbolsCount} фруктов, например &#127826, после чего необходимо отметить запомненные вами цифры.
        </div>`);
    }
})

let $startButton = $("#start-button").on('click',()=>{
    if(!$('#respobdent-name').val()){
        alert("Укажите свое имя");
        return
    }
    if($('.form-select').val()==='0'){
        alert("Укажите тип проверки");
        return
    }
    if($('.form-select').val()==='1'){
        OutputRandomTable(romes);
    }else {
        OutputRandomTable(fruits,true);
    }
    SetDisable(true);
    setTimeout(Clear,1500)
})

function Clear(){
    let $checkField = $('#check-content');
    $checkField.empty();
    OutputAnswers()
}

function OutputAnswers(){
    let $checkField = $('#check-content');
    if($('.form-select').val()==='1'){
        OutputTable(romes);
    }else{
        OutputTable(fruits,true);
    }
    $checkField.append(`<div class="row"><div class="col"><button id="calc" class="btn btn-primary" type="button">Проверить</button></div></div>`);
    $('#calc').on('click',CheckAnswers);
    $('.answer').on('click', DisabledInput);
}

function OutputTable(arr, isFruits = false){
    let table = `<table class="table"><th>#</th><th></th><tbody>`
    for(let i = 0;i<arr.length;i++){
        table += `<tr>
                    <td>${arr[i]}</td>
                    <td>
                        <input class="answer form-check-input" type="checkbox" id='${isFruits ? arr[i].slice(2):arr[i]}'>
                    </td>
                </tr>`
    }
    table += '</tbody></table>';
    $('#check-content').append(table);
}

function OutputRandomTable(arr, isFruits = false){
    let table = `<table class="table"><th>#</th><tbody>`
    let cloneArr = arr.slice(0);
    let isRandomSize = $('#random-fontsize:checked').length !== 0; 
    for (let i = 0 ; (i < symbolsCount) && (i < cloneArr.length) ; i++) {
        let r = Math.floor(Math.random() * (cloneArr.length - i)) + i;
        let item = cloneArr[r];
        cloneArr[r] = cloneArr[i];
        cloneArr[i] = item;
        rightAnswerArr.push( isFruits ? item.slice(2):item);
        table +=  `<tr>
                        <td style='font-size:${GetFontSize(12,35,isRandomSize)}px'>${item}</td>
                    </tr>`
    }
    table +=`</tbody></table>`;
    $('#check-content').append(table);
}

function CheckAnswers(){
    $('#calc').remove();
    $('#check-content').append(`<div class="row"><div class="col"><button id="restart" class="btn btn-primary" type="button">Начать заново</button></div></div>`);
    $('#restart').on('click',Restart);
    let $answers = $('.answer:checked')
    for(var i=0;i< $answers.length;i++){
        respondentAnswer.push($answers[i].id);
    }

    console.log(respondentAnswer);
    console.log(rightAnswerArr);

    let rightCOuntAnswers = 0;
    let wrongCountAnswers = 0;
    for(let i = 0;i<respondentAnswer.length;i++){
        if(rightAnswerArr.includes(respondentAnswer[i]))
            rightCOuntAnswers++;
        else 
            wrongCountAnswers++;
    }

    alert(`Правильных ответов: ${rightCOuntAnswers} из ${symbolsCount}
    Неправильных ответов: ${wrongCountAnswers}`);
    SaveData(localStorage.length,{
        name:$('#respobdent-name').val(),
        rightCount: respondentAnswer.length,
        typeCheck: $('.form-select').val(),
        isRamdomSize: $('#random-fontsize:checked').length !== 0
    });
    respondentAnswer = []
    rightAnswerArr = []
}

function DisabledInput(){
    if($('.answer:checked').length===symbolsCount){
        $('.answer:checkbox:not(:checked)').prop('disabled', true)
    }else{
        $('.answer:checkbox:not(:checked)').prop('disabled', false)
    }
}

function Restart(){
    SetDisable(false);
    $('#check-content').empty();
}

function SetDisable(isDisable){
    $('#respobdent-name').prop('disabled', isDisable);
    $('#random-fontsize').prop('disabled', isDisable);
    $('.form-select').prop('disabled', isDisable);
    $('#reset-button').prop('disabled', isDisable);
    $startButton.prop('disabled', isDisable);
}