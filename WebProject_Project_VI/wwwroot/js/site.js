// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// script.js

function toggleAccordion(id) {
    const accordionBody = document.getElementById(id);
    accordionBody.style.display = (accordionBody.style.display === 'none' || accordionBody.style.display === '') ? 'block' : 'none';


}